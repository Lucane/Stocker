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

    public async Task<List<WarehouseConnections>> GetWarehouseConnections()
    {
        // Get Weather Forecasts  
        return await _context.WarehouseConnections.AsNoTracking().ToListAsync();
    }
}