using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LT.DigitalOffice.NewsService.Models.Db
{
    public class DbNews
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime PostTime { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public Guid UserId { get; set; }
        public ICollection<DbNewsFile> FileIds { get; set; }
    }
}
