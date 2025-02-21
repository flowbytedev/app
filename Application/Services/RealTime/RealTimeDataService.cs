using Application.Data;
using Application.Shared.Models.RealTime;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.RealTime;

public class RealTimeDataService : IRealTimeDataService
{
    private readonly IMemoryCache _cache;
    private const string CacheKey = "SalesData";
    private readonly ApplicationDbContext _context;

    public RealTimeDataService(IMemoryCache cache, ApplicationDbContext context)
    {
        _cache = cache;
        _context = context;
    }


    public List<SalesLineRealTime> GetAllSalesData()
    {
        var dataInCache = _cache.Get<ConcurrentDictionary<(string, DateTime), SalesLineRealTime>>(CacheKey);
        return dataInCache?.Values.ToList() ?? new List<SalesLineRealTime>();
    }



    public async Task AddSalesLineRealTimeToDb(List<SalesLineRealTime> salesLines)
    {
        await _context.SalesLineRealTime.AddRangeAsync(salesLines);
        await _context.SaveChangesAsync();

        //foreach (var salesLine in salesLines)
        //{
        //    //var existingSalesLine = await _context.SalesLineRealTime.FindAsync(salesLine.DateTime, salesLine.StoreCode);

        //    //if (existingSalesLine == null)
        //    //{
        //    //    await _context.SalesLineRealTime.AddAsync(salesLine);
        //    //}
        //    //else
        //    //{
        //    //    existingSalesLine.NetAmountAcy = salesLine.NetAmountAcy;
        //    //}

        //    await _context.SalesLineRealTime.AddAsync(salesLine);
        //    await _context.SaveChangesAsync();
        //}
    }



}
