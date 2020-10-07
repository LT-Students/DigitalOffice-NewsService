using System;

namespace LT.DigitalOffice.NewsService.Models.Dto
{
    public class CreateNewsRequest
    {
        public string Content { get; set; }
        public string Subject { get; set; }
        public string AuthorName { get; set; }
        public Guid AuthorId { get; set; }
        public Guid SenderId { get; set; }
    }
}
