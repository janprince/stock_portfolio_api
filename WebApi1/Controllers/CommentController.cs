using Microsoft.AspNetCore.Mvc;
using WebApi1.Dtos.Comment;
using WebApi1.Interfaces;
using WebApi1.Mappers;

namespace WebApi1.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;
    private readonly IStockRepository _stockRepo;

    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
    {
        _commentRepo = commentRepo;
        _stockRepo = stockRepo;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _commentRepo.GetAllAsync();
        var commentsDto = comments.Select(c => c.ToCommentDto());
        return Ok(commentsDto);
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var comment = await _commentRepo.GetByIdAsync(id);

        if (comment is null)
        {
            return NotFound();
        }

        return Ok(comment.ToCommentDto());

    }
    
    [HttpPost]
    [Route("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
    {
        // check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // check if the stock exists
        if (!await _stockRepo.StockExists(stockId))
        {
            return BadRequest("stock does not exist");
        }

        var commentModel = commentDto.ToCommentFromCreate(stockId);
        Console.WriteLine("==========================" + commentModel.Id.ToString());

        await _commentRepo.CreateAsync(commentModel);

        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
    {
        // check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var commentModel = await _commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdate());
        
        if (commentModel is null)
        {
            return NotFound("Comment not found");
        }

        return Ok(commentModel.ToCommentDto());

    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var commentModel = await _commentRepo.DeleteAsync(id);

        if (commentModel is null)
        {
            return NotFound("comment with Id not found");
        }

        return NoContent();
    }
    
}