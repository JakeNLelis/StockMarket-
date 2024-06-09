using System.Security.Cryptography;
using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var comments = await _commentRepo.GetAllCommentsAsync();

            var commentsList = comments.Select(c => c.ToCommentDto());

            return Ok(commentsList);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetComment([FromRoute] int id)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var comment = await _commentRepo.GetCommentByIdAsync(id);

            return comment == null ? NotFound() : Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentRequest commentRequest)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            if(!await _stockRepo.StockExist(stockId)) return BadRequest("Stock does not exist");

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            
            var comment = commentRequest.FromCommentDtoToComment(stockId, appUser);

            await _commentRepo.CreateAsync(comment);

            return CreatedAtAction(nameof(GetComment), new {id = comment.Id}, comment.ToCommentDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequest commentRequest)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var commnetModel = await _commentRepo.UpdateAsync(id, commentRequest);

            return commnetModel == null ? NotFound() : Ok(commnetModel.ToCommentDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var commentModel = await _commentRepo.DeleteAsync(id);

            return commentModel == null ? NotFound() : NoContent();
        }
    }
}