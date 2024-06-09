using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        public Task<List<Comment>> GetAllCommentsAsync();
        public Task<Comment?> GetCommentByIdAsync(int id);
        public Task<Comment> CreateAsync(Comment comment);
        public Task<Comment?> UpdateAsync(int id, UpdateCommentRequest comment);
        public Task<Comment?> DeleteAsync(int id);
    }
}