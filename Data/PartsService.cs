using StockerDB.Data.Stocker;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stocker.Parts.Lenovo;

namespace Stocker.Data;
public class PartsService
{

    //public Task<WarehouseConnections[]> GetWarehouseConnections()
    //{
    //    //return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WarehouseConnections { }).AsNoTracking().ToListAsync();
    //}

    private readonly StockerContext _context;
    public PartsService(StockerContext context)
    {
        _context = context;
    }

    public async Task<List<LenovoDevices>> GetAllProductsAsync()
    {
        return await _context.LenovoDevices.AsNoTracking().ToListAsync();
    }

    public async Task<bool> PopulateDevicesAsync()
    {
        SessionStorage caller_Devices = new SessionStorage(_context);
        var result = await caller_Devices.UpdateDevices();

        return result;
    }
}