using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Stocker.Data;
using StockerDB.Data.Stocker;
//using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;

namespace Stocker.API.ACC;
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

    public async Task<string?> RequestApi()
    {
        // [REVIEW] Reading API response from file is for testing purposes only. Must be removed from production code!
        //var contents = File.ReadAllText(@"C:\Users\Avalerion\Desktop\LZ4\ACC_Products_ALL.txt");
        //return contents;

        var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.accdistribution.net/");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var query = "{\"request\":{\"LicenseKey\":\"" + _config["ApiKeys:ACC:licenseKey"] +
                    "\",\"Locale\":\"en\",\"Currency\":\"EUR\",\"CompanyId\":\"_al\",\"Limit\":\"25000\",\"Filters\":[{\"id\":\"producer\",\"values\":[\"LV\"]}]}}";

        var request = new HttpRequestMessage(HttpMethod.Post, "v1/GetProducts");
        request.Content = new StringContent(query, Encoding.UTF8, "application/json");

        try {
            var response = await client.SendAsync(request);
            System.Diagnostics.Debug.WriteLine($@"Response from ACC API :: {response}");

            if (response is not null && response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
            
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while querying ACC API :: {ex.Message} &-& {ex.InnerException}");
        }

        return null;
    }

    public async Task<bool> PopulateDatabase(string content)
    {
        if (String.IsNullOrWhiteSpace(content)) return await Task.FromResult(false);

        Rootobject responseObject = null;

        try {
            responseObject = JsonSerializer.Deserialize<Rootobject>(content);

        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while querying ACC API :: {ex.Message} &-& {ex.InnerException}");
        }

        if (responseObject is null) return await Task.FromResult(false);

        var allProducts = new List<WarehouseProducts>();

        await _context.Database.ExecuteSqlRawAsync("DELETE FROM dbo.WarehouseProducts WHERE Warehouse='ACC'");
        var updatedAt = DateTime.Now;

        foreach (var product in responseObject.Products) {
            var newProduct = new WarehouseProducts();

            newProduct.CarePack = Convert.ToByte(product.IsEsd);
            newProduct.Category = product.Branches.FirstOrDefault()?.Name;
            newProduct.Date_Incoming = product.Stocks.Where(x => x.WhId == null && x.AmountOrderedArrivingDiff > (decimal)0.0).FirstOrDefault()?.ExpectedDate;
            newProduct.Description = product.Name;
            newProduct.LastUpdated = updatedAt;
            newProduct.PartNumber = product.MPN;
            newProduct.Price_Local = product.Price.Value;
            newProduct.Stock_Incoming = Convert.ToInt32(product.Stocks.Where(x => x.WhId == null && x.AmountOrderedArrivingDiff > (decimal)0.0).FirstOrDefault()?.AmountOrderedArrivingDiff);
            newProduct.Stock_Local = Convert.ToInt32(product.Stocks.Where(x => x.WhId == "SALES").FirstOrDefault()?.Amount);

            if (product.Reserve is not null) {
                newProduct.Stock_Ordered = Convert.ToInt32(product.Reserve.OrderQty);
                newProduct.Stock_Reserved = Convert.ToInt32(product.Reserve.ReserveQty);
            }
            
            newProduct.Warehouse = "ACC";
            newProduct.Warranty = product.Warranty + " months";

            _context.Add(newProduct);
        }

        _context.SaveChanges();

        return await Task.FromResult(true);
    }
}
