using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
    public class CreateCommentRequest
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title Must be more than 4 characters!")]
        [MaxLength(300, ErrorMessage = "Title cannot be over 300 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(10, ErrorMessage = "Title Must be more than 4 characters!")]
        [MaxLength(500, ErrorMessage = "Title cannot be over 300 characters")]
        public string Content { get; set; } = string.Empty;
    }
}