﻿using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using StockerDB.Data.Stocker;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components;

namespace Stocker.API.ALSO;
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
        //var contents = File.ReadAllText(@"C:\Users\Avalerion\Desktop\ALSO_data.xml");
        //return contents;

        var client = new HttpClient();

        var request = XDocument.Parse(@"<?xml version=""1.0"" encoding=""UTF - 8""?><CatalogRequest>" +
                @"<Route><From><ClientID>" + _config["ApiKeys:ALSO:clientId"] + "</ClientID></From><To><ClientID>0</ClientID></To></Route>" +
                @"<Filters><Filter FilterID=""VendorID"" Value=""80008028"" /><Filter FilterID=""StockLevel"" Value=""All"" />" +
                @"<Filter FilterID=""Price"" Value=""WOVAT"" />" +
                @"</Filters></CatalogRequest>");

        try {
            var query = $@"https://b2b.also.ee/DirectXML.svc/0/scripts/XML_Interface.dll?USERNAME={_config["ApiKeys:ALSO:username"]}&PASSWORD={_config["ApiKeys:ALSO:password"]}&XML={request}";
            var response = await client.GetAsync(query);
            System.Diagnostics.Debug.WriteLine($@"Response from ALSO API :: {response}");
            System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());

            if (response is not null && response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($@"ERROR while querying ALSO API :: {ex.Message} &-& {ex.InnerException}");
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
            System.Diagnostics.Debug.WriteLine($@"ERROR while deserializing ALSO API response :: {ex.Message} &-& {ex.InnerException}");
        }

        if (responseObject is null) return await Task.FromResult(false);

        var lastUpdated = DateTime.Now;
        var allProducts = new List<WarehouseProducts>();

        await _context.Database.ExecuteSqlRawAsync("DELETE FROM dbo.WarehouseProducts WHERE Warehouse='ALSO'");

        foreach (var product in responseObject.ListofCatalogDetails) {
            //if (product.Producer.OId != "LV") continue;

            var newProduct = new WarehouseProducts();

            newProduct.CarePack = product.Product.CarePack;
            newProduct.Category = product.Product.Grouping.Where(x => x.GroupID == "ClassID").FirstOrDefault()?.Value;
            newProduct.Description = product.Product.LongDesc;
            newProduct.LastUpdated = lastUpdated;
            newProduct.PartNumber = product.Product.PartNumber;
            newProduct.Price_Local = product.Price.UnitPrice.Value;
            newProduct.Price_Remote = product.Qty.Where(x => x.WarehouseID == "FI" || x.WarehouseID == "LT").Min(x => (decimal?)x.UnitPrice);
            newProduct.Stock_Local = Convert.ToInt32(product.Qty.Where(x => x.WarehouseID == "1").FirstOrDefault()?.QtyAvailable);
            newProduct.Stock_Remote = Convert.ToInt32(product.Qty.Where(x => x.WarehouseID == "2").FirstOrDefault()?.QtyAvailable);

            newProduct.Warehouse = "ALSO";
            newProduct.Warranty = product.Product.PeriodofWarranty;

            _context.Add(newProduct);
        }

        _context.SaveChanges();
        return await Task.FromResult(true);
    }
}
