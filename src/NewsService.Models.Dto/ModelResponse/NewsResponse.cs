using LT.DigitalOffice.NewsService.Models.Dto.Model;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.NewsService.Models.Dto.ModelResponse
{
    public class NewsResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public User Author { get; set; }
        public User Sender { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
