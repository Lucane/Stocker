using StockerDB.Data.Stocker;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;


namespace Stocker.Data;
public class WarehouseConnectionsService
{

    //public Task<WarehouseConnections[]> GetWarehouseConnections()
    //{
    //    //return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WarehouseConnections { }).AsNoTracking().ToListAsync();
    //}

    private readonly StockerContext _context;
    public WarehouseConnectionsService(StockerContext context)
    {
        _context = context;
    }

    public async Task<List<WarehouseConnections>> GetWarehousesAsync()
    {
        return await _context.WarehouseConnections.AsNoTracking().ToListAsync();
    }

    public Task<WarehouseConnections> CreateWarehouseAsync(WarehouseConnections objWarehouseConnection)
    {
        _context.WarehouseConnections.Add(objWarehouseConnection);
        _context.SaveChanges();
        return Task.FromResult(objWarehouseConnection);
    }

    public Task<bool> UpdateWarehouseAsync(WarehouseConnections objWarehouse)
    {
        var existingWarehouse = _context.WarehouseConnections
                                              .Where(x => x.Id == objWarehouse.Id)
                                              .FirstOrDefault();
        if (existingWarehouse != null) {
            existingWarehouse.DisplayName = objWarehouse.DisplayName;
            existingWarehouse.ConnectionUri = objWarehouse.ConnectionUri;
            existingWarehouse.LoginName = objWarehouse.LoginName;
            existingWarehouse.LoginSecret = objWarehouse.LoginSecret;
            existingWarehouse.LastUpdated = System.DateTime.Now;

            _context.SaveChanges();
        } else {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }

    public Task<bool> DeleteWarehouseAsync(WarehouseConnections objWarehouse)
    {
        var objExistingWarehouse = _context.WarehouseConnections
                                           .Where(x => x.Id == objWarehouse.Id)
                                           .FirstOrDefault();
        if (objExistingWarehouse != null) {
            _context.WarehouseConnections.Remove(objExistingWarehouse);
            _context.SaveChanges();
        } else {
            return Task.FromResult(false);
        }
        return Task.FromResult(true);
    }
}