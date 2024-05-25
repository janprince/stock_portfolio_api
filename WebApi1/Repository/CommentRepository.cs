using Microsoft.EntityFrameworkCore;
using WebApi1.Data;
using WebApi1.Dtos.Comment;
using WebApi1.Interfaces;
using WebApi1.Models;

namespace WebApi1.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        
        return comment ?? null;
    }

    public async Task<Comment> CreateAsync(Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();

        return commentModel;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment commentDto)
    {
        var existingComment = await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existingComment is null)
        {
            return null;
        }

        existingComment.Title = commentDto.Title;
        existingComment.Content = commentDto.Content;

        await _context.SaveChangesAsync();

        return existingComment;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var existingComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

        if (existingComment is null)
        {
            return null;
        }

        _context.Comments.Remove(existingComment);
        await _context.SaveChangesAsync();

        return existingComment;
    }
}