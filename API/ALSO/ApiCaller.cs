using System.Net.Http.Headers;
using System.Text;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Stocker.Data;
using StockerDB.Data.Stocker;
using System.Xml.Linq;
//using Newtonsoft.Json;

namespace Stocker.API.ALSO;
public class ApiCaller
{
    //private readonly IDbContextFactory<ApplicationDbContext> _context;

    //public ApiCaller(IDbContextFactory<ApplicationDbContext> contextFactory)
    //{
    //    _context = contextFactory;
    //}

    private readonly StockerContext _context;
    public ApiCaller(StockerContext context)
    {
        _context = context;
    }

    public async Task<bool> GetProducts()
    {
        var apiResponse = await RequestApi();
        if (apiResponse is null) return await Task.FromResult(false);

        var populateResponse = await PopulateDatabase(apiResponse);
        if (populateResponse is false) return await Task.FromResult(false);

        return await Task.FromResult(true);
    }

    public async Task<string?> RequestApi()
    {
        // [REVIEW] Reading API response from file is for testing purposes only. Must be removed from production code!
        //var contents = File.ReadAllText(@"C:\Users\Avalerion\Desktop\ALSO_data.xml");
        //return contents;

        var client = new HttpClient();
        //HttpResponseMessage response = new HttpResponseMessage();
        string query;

        var request = XDocument.Parse(@"<?xml version=""1.0"" encoding=""UTF - 8""?><CatalogRequest>" +
                @"<Route><From><ClientID>***REMOVED***</ClientID></From><To><ClientID>0</ClientID></To></Route>" +
                @"<Filters><Filter FilterID=""VendorID"" Value=""80008028"" /><Filter FilterID=""StockLevel"" Value=""Transit"" />" +
                @"<Filter FilterID=""Price"" Value=""WOVAT"" />" +
                @"</Filters></CatalogRequest>");

        try {
            query = Uri.EscapeUriString($@"https://b2b.also.ee/DirectXML.svc/0/scripts/XML_Interface.dll?USERNAME=***REMOVED***&PASSWORD=***REMOVED***&XML={request}");
            var response = await client.GetAsync(query);
            System.Diagnostics.Debug.WriteLine($@"Response from ALSO API :: {response}");

            if (response is not null && response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

            //await client.SendAsync(request)
            //        .ContinueWith(responseTask => {
            //            System.Diagnostics.Debug.WriteLine($@"Response from ACC API :: ({responseTask.Status})");
            //            System.Diagnostics.Debug.WriteLine("RESULT :: " + responseTask.Result);
            //            System.Diagnostics.Debug.WriteLine("CONTENT :: " + responseTask.Result.Content.ReadAsStringAsync().Result);
            //        });

            //System.Diagnostics.Debug.WriteLine("finished processing API");
            //return null;
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while querying ALSO API :: {ex.Message} &-& {ex.InnerException}");
        }

        return null;
    }

    public async Task<bool> PopulateDatabase(string content)
    {
        if (String.IsNullOrWhiteSpace(content)) return await Task.FromResult(false);

        PriceCatalog responseObject = null;

        try {
            var serializer = new XmlSerializer(typeof(PriceCatalog));
            using (TextReader reader = new StringReader(content)) {
                responseObject = (PriceCatalog)serializer.Deserialize(reader);
            }
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while querying ALSO API :: {ex.Message} &-& {ex.InnerException}");
        }
        //var responseObject = JsonConvert.DeserializeObject<Root>(content);

        if (responseObject is null) return await Task.FromResult(false);

        var lastUpdated = DateTime.Now;
        var allProducts = new List<WarehouseProducts>();

        await _context.Database.ExecuteSqlRawAsync("DELETE FROM dbo.WarehouseProducts WHERE Warehouse='ALSO'");

        foreach (var product in responseObject.ListofCatalogDetails) {
            //if (product.Producer.OId != "LV") continue;

            var newProduct = new WarehouseProducts();

            newProduct.CarePack = product.Product.CarePack;
            newProduct.Category = product.Product.Grouping.Where(x => x.GroupID == "ClassID").FirstOrDefault()?.Value;
            //newProduct.Date_Incoming = product.
            newProduct.Description = product.Product.LongDesc;
            newProduct.LastUpdated = lastUpdated;
            newProduct.PartNumber = product.Product.PartNumber;
            newProduct.Price_Local = product.Price.UnitPrice.Value;
            newProduct.Price_Remote = product.Qty.Where(x => x.WarehouseID == "FI" || x.WarehouseID == "LT").Min()?.UnitPrice;
            //newProduct.Stock_Incoming = Convert.ToInt32(product.Qty.Where(x => x.WarehouseID == "1").FirstOrDefault().QtyAvailable);
            newProduct.Stock_Local = Convert.ToInt32(product.Qty.Where(x => x.WarehouseID == "1").FirstOrDefault()?.QtyAvailable);
            newProduct.Stock_Remote = Convert.ToInt32(product.Qty.Where(x => x.WarehouseID == "2").FirstOrDefault()?.QtyAvailable);

            //if (product.Reserve is not null) {
            //    newProduct.Stock_Ordered = Convert.ToInt32(product.Reserve.OrderQty);
            //    newProduct.Stock_Reserved = Convert.ToInt32(product.Reserve.ReserveQty);
            //}

            newProduct.Warehouse = "ALSO";
            newProduct.Warranty = product.Product.PeriodofWarranty;

            _context.Add(newProduct);
        }

        _context.SaveChanges();
        return await Task.FromResult(true);
    }
}
