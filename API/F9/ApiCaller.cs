using System.Text;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using StockerDB.Data.Stocker;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components;

namespace Stocker.API.F9;
public class ApiCaller
{
    private readonly StockerContext _context;
    public ApiCaller(StockerContext context)
    {
        _context = context;
    }

    [Inject]
    private IConfiguration _config { get; set; }

    public async Task<bool> GetProducts()
    {
        var apiResponse = await RequestApi();
        if (apiResponse is null) return await Task.FromResult(false);

        var populateResponse = await PopulateDatabase(apiResponse);
        if (populateResponse is false) return await Task.FromResult(false);

        return await Task.FromResult(true);
    }

    public static string GenerateMD5(string input)
    {

        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++) {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    public async Task<string?> RequestApi()
    {
        // [REVIEW] Reading API response from file is for testing purposes only. Must be removed from production code!
        //var contents = File.ReadAllText(@"C:\Users\Avalerion\Desktop\LZ4\F9_PriceCatalog_FULL.txt");
        //return contents;

        var client = new HttpClient();
        string query;

        var request = XDocument.Parse("<CatalogRequest>" +
                            $"<Date>{DateTime.Now.ToString("yyyy-MM-ddTHH-mm-sszzz")}</Date>" +
                            "<Route>" +
                                "<From>" +
                                    "<ClientID>" + _config["ApiKeys:F9:clientId"] + "</ClientID>" +
                                "</From>" +
                                "<To>" +
                                    "<ClientID>1</ClientID>" +
                                "</To>" +
                            "</Route>" +
                            "<Filters>" +
                                "<Filter FilterID=\"Price\" Value=\"WOVAT\"/>" +
                                "<Filter FilterID=\"VendorID\" Value=\"LEN\"/>" +
                            "</Filters>" +
                        "</CatalogRequest>");

        var hash = GenerateMD5(request + "85986593409238459803");
        query = $@"http://www.f9baltic.com/scripts/XML_Interface.dll?MfcISAPICommand=Default&USERNAME={_config["ApiKeys:F9:username"]}&PASSWORD={_config["ApiKeys:F9:password"]}&XML={request}&CHECK={hash}";

        try {
            var response = await client.GetAsync(query);
            System.Diagnostics.Debug.WriteLine($@"Response from F9 API :: {response}");

            if (response is not null && response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while querying F9 API :: {ex.Message} &-& {ex.InnerException}");
        }

        return null;
    }

    public async Task<bool> PopulateDatabase(string content)
    {
        if (String.IsNullOrWhiteSpace(content)) return await Task.FromResult(false);

        PriceCatalog? responseObject = null;

        try {
            var serializer = new XmlSerializer(typeof(PriceCatalog));
            using (TextReader reader = new StringReader(content)) {
                responseObject = (PriceCatalog)serializer.Deserialize(reader);
            }
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while deserializing F9 API response :: {ex.Message} &-& {ex.InnerException}");
        }

        if (responseObject is null) return await Task.FromResult(false);

        var lastUpdated = DateTime.Now;
        var allProducts = new List<WarehouseProducts>();

        await _context.Database.ExecuteSqlRawAsync("DELETE FROM dbo.WarehouseProducts WHERE Warehouse='F9'");

        foreach (var product in responseObject.ListofCatalogDetails) {
            var newProduct = new WarehouseProducts();
            var dateIncoming = product.Qty.Where(x => x.QtyAvailable == 0)
                                          .Min(x => (DateTime?)x.DeliveryDate);

            newProduct.CarePack = Convert.ToByte(product.Qty.Any(x => x.QtyAvailable >= 10000));
            if (dateIncoming != DateTime.MinValue) newProduct.Date_Incoming = dateIncoming;
            newProduct.Description = product.Product.Description;
            newProduct.LastUpdated = lastUpdated;
            newProduct.PartNumber = product.Product.PartNumber;
            newProduct.Price_Local = product.Price.UnitPrice.FirstOrDefault()?.Value;
            newProduct.Stock_Local = product.Qty.Sum(x => x.QtyAvailable);

            newProduct.Warehouse = "F9";
            newProduct.Warranty = product.Product.PeriodofWarranty;

            _context.Add(newProduct);
        }

        _context.SaveChanges();
        return await Task.FromResult(true);
    }
}
