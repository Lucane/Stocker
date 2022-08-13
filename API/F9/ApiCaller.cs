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

namespace Stocker.API.F9;
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

    //public async Task<bool> GetProducts()
    //{
    //    var apiResponse = await RequestApi();
    //    if (apiResponse is null) return await Task.FromResult(false);

    //    var populateResponse = await PopulateDatabase(apiResponse);
    //    if (populateResponse is false) return await Task.FromResult(false);

    //    return await Task.FromResult(true);
    //}

    public async Task<string?> RequestApi()
    {
        // [REVIEW] Reading API response from file is for testing purposes only. Must be removed from production code!
        //var contents = File.ReadAllText(@"C:\Users\Avalerion\Desktop\ALSO_data.xml");
        //return contents;

        var client = new HttpClient();
        HttpResponseMessage response = new HttpResponseMessage();
        string query;

        var request = XDocument.Parse("<CatalogRequest>" +
                                                "<Date>+2011-10-12T14:58:47-05:00</Date>" +
                                                "<Route>" +
                                                    "<From>" +
                                                        "<ClientID>***REMOVED***</ClientID>" +
                                                    "</From>" +
                                                    "<To>" +
                                                        "<ClientID>1</ClientID>" +
                                                    "</To>" +
                                                "</Route>" +
                                                "<Filters>" +
                                                    "<Filter FilterID=\"Price\" Value=\"WOVAT\"/>" +
                                                "</Filters>" +
                                            "</CatalogRequest>");

        query = Uri.EscapeUriString($@"http://www.f9.fi/scripts/XML_Interface.dll?USERNAME=aigorkuuse&PASSWORD=l2tainas16&XML={request}");
        response = await client.GetAsync(query);

        return await response.Content.ReadAsStringAsync();

        /*if (response != null && response.IsSuccessStatusCode) {
            System.Diagnostics.Debug.WriteLine($@"Response from F9 API :: ({response.StatusCode}) - {response.ReasonPhrase}");
            return await response.Content.ReadAsStringAsync();
            //var formattedXml = FormatXml(response.Content.ReadAsStringAsync().Result);
        } else {
            System.Diagnostics.Debug.WriteLine($@"Response from F9 API :: ({response.StatusCode}) - {response.ReasonPhrase}");
            return null;
            //await File.WriteAllTextAsync($@"{Globals.EXE_PATH}\XML\products_also.log", $"{(int)response.StatusCode} ({response.ReasonPhrase})");
        }*/

        /*var client = new HttpClient();
        client.BaseAddress = new Uri("http://www.f9.fi/");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var request = new HttpRequestMessage(HttpMethod.Post, "scripts/XML_Interface.dll?USERNAME=aigorkuuse&PASSWORD=l2tainas16");
        request.Content = new StringContent("<CatalogRequest>" +
                                                "<Date>+2011-10-12T14:58:47-05:00</Date>" +
                                                "<Route>" +
                                                    "<From>" +
                                                        "<ClientID>***REMOVED***</ClientID>" +
                                                    "</From>" +
                                                    "<To>" +
                                                        "<ClientID>1</ClientID>" +
                                                    "</To>" +
                                                "</Route>" +
                                                "<Filters>" +
                                                    "<Filter FilterID=\"Price\" Value=\"WOVAT\"/>" +
                                                "</Filters>" +
                                            "</CatalogRequest>", Encoding.UTF8, "application/json");

        try {
            var response = await client.SendAsync(request);
            System.Diagnostics.Debug.WriteLine($@"Response from F9 API :: {response}");

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
            System.Diagnostics.Debug.WriteLine($@"ERROR while querying F9 API :: {ex.Message} &-& {ex.InnerException}");
        }

        return null;*/
    }

    /*public async Task<bool> PopulateDatabase(string content)
    {
        if (String.IsNullOrWhiteSpace(content)) return await Task.FromResult(false);

        PriceCatalog responseObject = null;

        try {
            var serializer = new XmlSerializer(typeof(PriceCatalog));
            using (TextReader reader = new StringReader(content)) {
                responseObject = (PriceCatalog)serializer.Deserialize(reader);
            }
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while querying F9 API :: {ex.Message} &-& {ex.InnerException}");
        }
        //var responseObject = JsonConvert.DeserializeObject<Root>(content);

        if (responseObject is null) return await Task.FromResult(false);

        var lastUpdated = DateTime.Now;
        var allProducts = new List<WarehouseProducts>();

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
            newProduct.Price_Remote = product.Qty.Where(x => x.WarehouseID == "FI" || x.WarehouseID == "LT").FirstOrDefault()?.UnitPrice;
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
    }*/
}
