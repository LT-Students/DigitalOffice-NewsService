using LT.DigitalOffice.NewsService.Models.Dto.Models;
using System;

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
