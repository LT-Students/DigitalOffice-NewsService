using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.NewsService.Models.Db
{
    public class DbNews
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Subject { get; set; }
        public string AuthorName { get; set; }
        [Required]
        public Guid AuthorId { get; set; }
        [Required]
        public Guid SenderId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}