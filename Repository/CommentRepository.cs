using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }   

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await GetCommentByIdAsync(id);
            if (commentModel == null) return null;
            _context.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            
            return await _context.Comments.Include(c => c.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.Include(c => c.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentRequest comment)
        {
            var commentModel = await GetCommentByIdAsync(id);
            if(commentModel == null) return null;

            commentModel.Id = commentModel.Id;
            commentModel.Title = comment.Title;
            commentModel.Content = comment.Content;
            commentModel.CreatedOn = commentModel.CreatedOn;
            commentModel.StockId = commentModel.StockId;

            await _context.SaveChangesAsync();
            return commentModel;
        }
    }
}