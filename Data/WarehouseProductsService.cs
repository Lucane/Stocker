using System.Xml.Linq;
using StockerDB.Data.Stocker;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace Stocker.Data;
public class WarehouseProductsService
{

    //public Task<WarehouseConnections[]> GetWarehouseConnections()
    //{
    //    //return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WarehouseConnections { }).AsNoTracking().ToListAsync();
    //}

    private readonly StockerContext _context;
    public WarehouseProductsService(StockerContext context)
    {
        _context = context;
    }

    public async Task<List<WarehouseProducts>> GetAllProductsAsync()
    {
        return await _context.WarehouseProducts.AsNoTracking().ToListAsync();
    }

    public async Task<bool> PopulateDatabaseAsync()
    {
        //var xDoc = await Xml.GetXmlRequest();
        var xDoc = XDocument.Load(@"C:\Users\Avalerion\Desktop\ALSO_data.xml");
        if (String.IsNullOrWhiteSpace(xDoc?.ToString())) return await Task.FromResult(false);
        
        var products = xDoc.Element("PriceCatalog")?.Element("ListofCatalogDetails")?.Elements("CatalogItem");
        if (products == null || products.Count() == 0) await Task.FromResult(false);

        //var affectedRows = await _context.Database.ExecuteSqlRawAsync("delete from dbo.WarehouseProducts_ALSO");
        //if (affectedRows <= 0) return await Task.FromResult(false);

        var lastUpdatedParse = DateTime.TryParse(xDoc.Element("PriceCatalog")?.Element("PriceCatHdr")?.Element("Date")?.Value, out DateTime lastUpdated);

        foreach (var row in products!) {
            var newProduct = new WarehouseProducts_ALSO();

            newProduct.LastUpdated = lastUpdated;
            System.Diagnostics.Debug.WriteLine(newProduct.LastUpdated);
            newProduct.PartNumber = row.Element("Product")?.Element("PartNumber")?.Value;
            System.Diagnostics.Debug.WriteLine(newProduct.PartNumber);
            newProduct.Description = row.Element("Product")?.Element("LongDesc")?.Value;
            System.Diagnostics.Debug.WriteLine(newProduct.Description);
            newProduct.Warranty = row.Element("Product")?.Element("PeriodofWarranty")?.Value;
            System.Diagnostics.Debug.WriteLine(newProduct.Warranty);
            newProduct.CarePack = byte.Parse(row.Element("Product")?.Element("CarePack")?.Value ?? "-1");
            System.Diagnostics.Debug.WriteLine(newProduct.CarePack);

            if (Decimal.TryParse(row.Element("Price")?.Element("UnitPrice")?.Value
                                 , out var price_EE)) {
                newProduct.Price_EE = price_EE;
                System.Diagnostics.Debug.WriteLine("Price_EE: " + newProduct.Price_EE);
            }

            if (Decimal.TryParse(row.Elements("Qty")?
                                    .Where(x => x.Attribute("WarehouseID")?.Value == "FI")
                                    .FirstOrDefault()?.Element("UnitPrice")?.Value
                                 , out var Price_FI)) {
                newProduct.Price_FI = Price_FI;
                System.Diagnostics.Debug.WriteLine("Price_FI: " + newProduct.Price_FI);
            }

            if (Decimal.TryParse(row.Elements("Qty")?
                                    .Where(x => x.Attribute("WarehouseID")?.Value == "LT")
                                    .FirstOrDefault()?.Element("UnitPrice")?.Value
                                 , out var Price_LT)) {
                newProduct.Price_LT = Price_LT;
                System.Diagnostics.Debug.WriteLine("Price_LT: " + newProduct.Price_LT);
            }

            if (Int32.TryParse(row.Elements("Qty")?
                                            .Where(x => x.Attribute("WarehouseID")?.Value == "1")
                                            .FirstOrDefault()?.Element("QtyAvailable")?.Value
                               , out var Stock_EE)) {
                newProduct.Stock_EE = Stock_EE;
            }

            if (Int32.TryParse(row.Elements("Qty")?
                                            .Where(x => x.Attribute("WarehouseID")?.Value == "FI")
                                            .FirstOrDefault()?.Element("QtyAvailable")?.Value
                               , out var Stock_FI)) {
                newProduct.Stock_FI = Stock_FI;
            }

            if (Int32.TryParse(row.Elements("Qty")?
                                            .Where(x => x.Attribute("WarehouseID")?.Value == "LT")
                                            .FirstOrDefault()?.Element("QtyAvailable")?.Value
                               , out var Stock_LT)) {
                newProduct.Stock_LT = Stock_LT;
            }

            //newProduct.Price_EE = Decimal.Parse(row.Element("Price")?.Element("UnitPrice")?.Value ?? "-1");

            //newProduct.Price_FI = Decimal.Parse(row.Elements("Qty")?
            //                                .Where(x => x.Attribute("WarehouseID")?.Value == "FI")
            //                                .FirstOrDefault()?.Element("UnitPrice")?.Value ?? "-1");

            //newProduct.Price_LT = Decimal.Parse(row.Elements("Qty")?
            //                                .Where(x => x.Attribute("WarehouseID")?.Value == "LT")
            //                                .FirstOrDefault()?.Element("UnitPrice")?.Value ?? "-1");


            //newProduct.PriceLv = Int32.Parse(row.Elements("Qty")?
            //                                .Where(x => x.Attribute("WarehouseID")?.Value == "LV")
            //                                .FirstOrDefault()?.Value ?? "-1");

            //newProduct.Stock_EE = Int32.Parse(row.Elements("Qty")?
            //                                .Where(x => x.Attribute("WarehouseID")?.Value == "1")
            //                                .FirstOrDefault()?.Element("QtyAvailable")?.Value ?? "-1");
            //newProduct.Stock_FI = Int32.Parse(row.Elements("Qty")?
            //                                .Where(x => x.Attribute("WarehouseID")?.Value == "FI")
            //                                .FirstOrDefault()?.Element("QtyAvailable")?.Value ?? "-1");
            //newProduct.Stock_LT = Int32.Parse(row.Elements("Qty")?
            //                                .Where(x => x.Attribute("WarehouseID")?.Value == "LT")
            //                                .FirstOrDefault()?.Element("QtyAvailable")?.Value ?? "-1");
            //newProduct.StockLv = objProducts.StockLv;

            //newProduct.BidEndUser = objProducts.BidEndUser;
            //newProduct.BidMaxQty = objProducts.BidMaxQty;
            //newProduct.BidFreeQty = objProducts.BidFreeQty;
            //newProduct.BidPrice = objProducts.BidPrice;
            //newProduct.BidValidFrom = objProducts.BidValidFrom;
            //newProduct.BidValidTill = objProducts.BidValidTill;

            //await CreateProductAsync(newProduct);
            _context.WarehouseProducts_ALSO.Add(newProduct);
        }

        _context.SaveChanges();
        return await Task.FromResult(true);
    }

    public async Task<bool> PopDB()
    {
        //await _context.Database.ExecuteSqlRawAsync("delete from dbo.WarehouseProducts");

        API.ACC.ApiCaller caller_ACC = new API.ACC.ApiCaller(_context);
        //API.ALSO.ApiCaller caller_ALSO = new API.ALSO.ApiCaller(_context);

        //API.F9.ApiCaller caller_F9 = new API.F9.ApiCaller(_context);
        //var response = await caller_F9.RequestApi();
        //System.Diagnostics.Debug.WriteLine(response);

        await caller_ACC.GetProducts();
        //await caller_ALSO.GetProducts();

        //if (response_ACC) await _context.Database.ExecuteSqlRawAsync("DELETE FROM dbo.WarehouseProducts WHERE Warehouse = ACC");
        //if (response_ALSO) await _context.Database.ExecuteSqlRawAsync("DELETE FROM dbo.WarehouseProducts WHERE Warehouse = ALSO");

        return await Task.FromResult(true);
        //return await caller.GetProducts();
    }

    public Task<WarehouseProducts> CreateProductAsync(WarehouseProducts objWarehouseConnection)
    {
        _context.WarehouseProducts.Add(objWarehouseConnection);
        _context.SaveChanges();
        return Task.FromResult(objWarehouseConnection);
    }

    public Task<List<WarehouseProducts>> CreateProductsAsync(List<WarehouseProducts> objWarehouseProducts)
    {
        foreach (var product in objWarehouseProducts) {
            _context.WarehouseProducts.Add(product);
        }
        
        _context.SaveChanges();
        return Task.FromResult(objWarehouseProducts);
    }

    public Task<bool> UpdateProductAsync(WarehouseProducts_ALSO objProducts)
    {
        var existingProduct = _context.WarehouseProducts_ALSO
                                              .Where(x => x.Id == objProducts.Id)
                                              .FirstOrDefault();
        if (existingProduct != null) {
            //existingProduct.LastUpdated = DateTime.Now;

            existingProduct.LastUpdated = objProducts.LastUpdated;
            existingProduct.PartNumber = objProducts.PartNumber;
            existingProduct.Description = objProducts.Description;
            existingProduct.Warranty = objProducts.Warranty;
            existingProduct.CarePack = objProducts.CarePack;

            existingProduct.Price_EE = objProducts.Price_EE;
            existingProduct.Price_FI = objProducts.Price_FI;
            existingProduct.Price_LT = objProducts.Price_LT;
            existingProduct.Price_LV = objProducts.Price_LV;
            existingProduct.Stock_EE = objProducts.Stock_EE;
            existingProduct.Stock_FI = objProducts.Stock_FI;
            existingProduct.Stock_LT = objProducts.Stock_LT;
            existingProduct.Stock_LV = objProducts.Stock_LV;

            existingProduct.BidEndUser = objProducts.BidEndUser;
            existingProduct.BidMaxQty = objProducts.BidMaxQty;
            existingProduct.BidFreeQty = objProducts.BidFreeQty;
            existingProduct.BidPrice = objProducts.BidPrice;
            existingProduct.BidValidFrom = objProducts.BidValidFrom;
            existingProduct.BidValidTill = objProducts.BidValidTill;

            _context.SaveChanges();
        } else {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }

    public Task<bool> DeleteProductAsync(WarehouseProducts_ALSO objProducts)
    {
        var objExistingProducts = _context.WarehouseProducts_ALSO
                                           .Where(x => x.Id == objProducts.Id)
                                           .FirstOrDefault();
        if (objExistingProducts != null) {
            _context.WarehouseProducts_ALSO.Remove(objExistingProducts);
            _context.SaveChanges();
        } else {
            return Task.FromResult(false);
        }
        return Task.FromResult(true);
    }
}