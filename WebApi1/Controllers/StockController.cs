using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi1.Data;
using WebApi1.Dtos.Stock;
using WebApi1.Helpers;
using WebApi1.Interfaces;
using WebApi1.Mappers;
using WebApi1.Models;

namespace WebApi1.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IStockRepository _stockRepo;

    public StockController(ApplicationDbContext context, IStockRepository stockRepo)
    {
        _context = context;
        _stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
        var stocks = await _stockRepo.GetAllAsync(query);
        var stockDto = stocks.Select(s => s.ToStockDto());

        return Ok(stockDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var stock = await _stockRepo.GetByIdAsync(id);

        if (stock is null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        // check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var stockModel = stockDto.ToStockFromCreate();
        Console.WriteLine("===============================" + stockModel.Id.ToString());
        
        await _stockRepo.CreateAsync(stockModel);

        return CreatedAtAction(nameof(Get), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        // check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var stockModel = await _stockRepo.UpdateAsync(id, updateDto);

        if (stockModel is null)
        {
            return NotFound();
        }

        return Ok(stockModel.ToStockDto());
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        // check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var stock = await _stockRepo.DeleteAsync(id);

        if (stock is null)
        {
            return NotFound();
        }

        return NoContent();
    }
}