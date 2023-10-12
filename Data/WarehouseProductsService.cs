using System.Xml.Linq;
using StockerDB.Data.Stocker;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Stocker.Data;

public class WarehouseProductsService
{
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
        API.ACC.ApiCaller caller_ACC = new API.ACC.ApiCaller(_context);
        API.ALSO.ApiCaller caller_ALSO = new API.ALSO.ApiCaller(_context);
        API.F9.ApiCaller caller_F9 = new API.F9.ApiCaller(_context);

        await caller_F9.GetProducts();

        return await Task.FromResult(true);
    }

    public Task<WarehouseProducts> AddProductAsync(WarehouseProducts objWarehouseConnection)
    {
        _context.WarehouseProducts.Add(objWarehouseConnection);
        _context.SaveChanges();
        return Task.FromResult(objWarehouseConnection);
    }

    public Task<WarehouseProducts> RemoveProductAsync(WarehouseProducts objWarehouseConnection)
    {
        _context.WarehouseProducts.Remove(objWarehouseConnection);
        _context.SaveChanges();
        return Task.FromResult(objWarehouseConnection);
    }
}