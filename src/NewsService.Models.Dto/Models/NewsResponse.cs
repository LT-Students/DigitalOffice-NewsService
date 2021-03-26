using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.NewsService.Models.Dto.Models
{
    public class NewsResponse
    {
        public string Content { get; set; }
        public string Subject { get; set; }
        public string AuthorName { get; set; }
        public Guid AuthorId { get; set; } // Сheck with Spartack
        public Guid SenderId { get; set; } // Сheck with Spartack
        public DateTime CreatedAt { get; set; } // Сheck with Spartack
        public Guid DepartamentId { get; set; } // Сheck with Spartack
        public bool IsActive { get; set; } // Сheck with Spartack
    }
}
