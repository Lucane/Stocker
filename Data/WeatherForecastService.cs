using StockerDB.Data.Stocker;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;

namespace Stocker.Data;

public class WeatherForecastService
{
    private readonly StockerContext _context;
    public WeatherForecastService(StockerContext context)
    {
        _context = context;
    }
    public async Task<List<WeatherForecast>> GetForecastAsync(string strCurrentUser)
    {
        // Get Weather Forecasts  
        return await _context.WeatherForecast.Where(x => x.UserName == strCurrentUser).AsNoTracking().ToListAsync();
    }
}