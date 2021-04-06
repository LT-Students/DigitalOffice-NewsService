﻿using System;

namespace LT.DigitalOffice.NewsService.Models.Dto.Models
{
    public class News
    {
        public Guid? Id { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public string Pseudonym { get; set; }
        public Guid AuthorId { get; set; }
        public Guid SenderId { get; set; }
    }
}
