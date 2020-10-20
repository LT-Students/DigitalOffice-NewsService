using System;

namespace LT.DigitalOffice.NewsService.Models.Dto
{
    public class NewsRequest
    {
        public Guid? Id { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public string AuthorName { get; set; }
        public Guid AuthorId { get; set; }
        public Guid SenderId { get; set; }
    }
}
