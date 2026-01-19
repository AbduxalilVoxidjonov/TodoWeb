using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Models
{
    public class Todo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title kiritish shart")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? CompletedDate { get; set; }

        // User bilan bog'lash
        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}