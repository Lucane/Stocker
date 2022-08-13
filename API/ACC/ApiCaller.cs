using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Stocker.Data;
using StockerDB.Data.Stocker;
//using Newtonsoft.Json;

namespace Stocker.API.ACC;
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
        //var contents = File.ReadAllText(@"C:\Users\Avalerion\Desktop\LZ4\ACC_Products_ALL.txt");
        //return contents;

        var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.accdistribution.net/");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var request = new HttpRequestMessage(HttpMethod.Post, "v1/GetProducts");
        request.Content = new StringContent("{\"request\":{\"LicenseKey\":\"***REMOVED***\",\"Locale\":\"en\",\"Currency\":\"EUR\",\"CompanyId\":\"_al\",\"Limit\":\"25000\",\"Filters\":[{\"id\":\"producer\",\"values\":[\"LV\"]}]}}",
                            Encoding.UTF8, "application/json");         // ,\"Filters\":{\"producer\":\"LV\"}

        try {
            var response = await client.SendAsync(request);
            System.Diagnostics.Debug.WriteLine($@"Response from ACC API :: {response}");

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
            System.Diagnostics.Debug.WriteLine($@"ERROR while querying ACC API :: {ex.Message} &-& {ex.InnerException}");
        }

        return null;
    }

    public async Task<bool> PopulateDatabase(string content)
    {
        //var builder = WebApplication.CreateBuilder(args);

        //// Add services to the container.
        //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        //builder.Services.AddDbContext<ApplicationDbContext>(options =>
        //    options.UseSqlServer(connectionString));


        //var connection = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //Server=(localdb)\\mssqllocaldb;Database=aspnet-Stocker-6BAD2BE6-E604-46F3-8699-1B4A5DC57709
        //var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
        //    .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=aspnet-Stocker-6BAD2BE6-E604-46F3-8699-1B4A5DC57709")
        //    .Options;
        //using var dbContext = new ApplicationDbContext(contextOptions);
        //var optionsBuilder = new DbContextOptionsBuilder();
        //optionsBuilder.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]);
        //var dbContext = new StockerDB.Data.Stocker.StockerContext();
        //var context = new StockerDB.Data.Stocker.StockerContext(optionsBuilder.Options);

        //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        //builder.Services.AddDbContext<ApplicationDbContext>(options =>
        //    options.UseSqlServer(connectionString));



        //content = content.Replace(@"\""", @"\u0022");

        //var contentClean = HttpUtility.JavaScriptStringEncode(content);

        //JsonSerializerOptions jso = new() {
        //    AllowTrailingCommas = true,
        //    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        //};

        if (String.IsNullOrWhiteSpace(content)) return await Task.FromResult(false);

        Rootobject responseObject = null;

        try {
            responseObject = JsonSerializer.Deserialize<Rootobject>(content);

        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while querying ACC API :: {ex.Message} &-& {ex.InnerException}");
        }

        //var responseObject = JsonConvert.DeserializeObject<Root>(content);

        if (responseObject is null) return await Task.FromResult(false);

        var allProducts = new List<WarehouseProducts>();

        await _context.Database.ExecuteSqlRawAsync("DELETE FROM dbo.WarehouseProducts WHERE Warehouse='ACC'");

        foreach (var product in responseObject.Products) {
            if (product.Producer.OId != "LV") continue;

            var newProduct = new WarehouseProducts();

            newProduct.CarePack = Convert.ToByte(product.IsEsd);
            newProduct.Category = product.Branches.FirstOrDefault()?.Name;
            newProduct.Date_Incoming = product.Stocks.Where(x => x.WhId == null && x.AmountOrderedArrivingDiff > (decimal)0.0).FirstOrDefault()?.ExpectedDate;
            newProduct.Description = product.Name;
            newProduct.LastUpdated = product.UpdatedAt;
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
            //using (var context = _context.CreateDbContext()) {
            //    context.Add(newProduct);
            //}
            //dbContext.Add(newProduct);
            //dbContext.SaveChanges();
            //allProducts.Add(newProduct);
            //return allProducts;
            //.Database.Add(newProduct);
        }

        _context.SaveChanges();
        //using (var context = _context.CreateDbContext()) {
        //    context.SaveChanges();
        //}

        //dbContext.SaveChanges();
        return await Task.FromResult(true);
    }
}
