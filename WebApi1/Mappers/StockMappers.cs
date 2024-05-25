using WebApi1.Dtos.Stock;
using WebApi1.Models;

namespace WebApi1.Mappers;

public static class StockMappers
{
    public static StockDto ToStockDto(this Stock stockModel)
    {
        return new StockDto
        {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap,
            Comments = stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
        };
    }

    public static Stock ToStockFromCreate(this CreateStockRequestDto createData)
    {
        return new Stock
        {
            Symbol = createData.Symbol,
            CompanyName = createData.CompanyName,
            Purchase = createData.Purchase,
            LastDiv = createData.LastDiv,
            Industry = createData.Industry,
            MarketCap = createData.MarketCap
        };
    }
}