using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi1.Data;
using WebApi1.Dtos.Stock;
using WebApi1.Helpers;
using WebApi1.Interfaces;
using WebApi1.Models;

namespace WebApi1.Respository;

public class StockRepository: IStockRepository
{
    private readonly ApplicationDbContext _context;
    
    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Stock>> GetAllAsync(QueryObject query)
    {
        var stocks =   _context.Stocks
            .Include(c => c.Comments)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.CompanyName))
        {
            stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
        }

        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            }
        }
        
        // pagination
        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        Console.WriteLine("=========================================================before==========================");
        return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        var stock = await _context.Stocks
            .Include(c => c.Comments)
            .FirstOrDefaultAsync(s => s.Id == id);
        return stock;
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        Console.WriteLine("===============================" + stockModel.Id.ToString());
        await _context.Stocks.AddAsync(stockModel);
        Console.WriteLine("===============================" + stockModel.Id.ToString());
        await _context.SaveChangesAsync();
        Console.WriteLine("===============================" + stockModel.Id.ToString());

        return stockModel;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
    {
        var existingStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

        if (existingStock is null)
        {
            return null;
        }
        existingStock.Symbol = stockDto.Symbol;
        existingStock.CompanyName = stockDto.CompanyName;
        existingStock.Purchase = stockDto.Purchase;
        existingStock.LastDiv = stockDto.LastDiv;
        existingStock.Industry = stockDto.Industry;
        existingStock.MarketCap = stockDto.MarketCap;

        await _context.SaveChangesAsync();

        return existingStock;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

        if (stockModel is null)
        {
            return null;
        }

        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();

        return stockModel;
    }

    public Task<bool> StockExists(int id)
    {
        return _context.Stocks
            .AnyAsync(s => s.Id == id);
    }
}