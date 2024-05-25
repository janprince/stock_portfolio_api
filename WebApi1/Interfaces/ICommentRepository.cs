using WebApi1.Dtos.Comment;
using WebApi1.Models;

namespace WebApi1.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync();
    Task<Comment?> GetByIdAsync(int id);
    Task<Comment> CreateAsync(Comment commentModel);
    Task<Comment?> UpdateAsync(int id, Comment commentDto);

    Task<Comment?> DeleteAsync(int id);
}