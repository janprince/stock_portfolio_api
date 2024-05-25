using WebApi1.Dtos.Stock;
using WebApi1.Helpers;
using WebApi1.Models;

namespace WebApi1.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync(QueryObject query);
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock> CreateAsync(Stock stockModel);
    Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
    Task<Stock?> DeleteAsync(int id);

    Task<bool> StockExists(int id);
}