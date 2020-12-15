﻿using System;

namespace LT.DigitalOffice.NewsService.Models.Dto.Models
{
    public class News
    {
        public Guid? Id { get; set; }
        public Guid AuthorId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public string AuthorName { get; set; }
    }
}
