using System;
using System.Collections.Generic;
using System.Text;

namespace LT.DigitalOffice.NewsService.Models.Dto
{
    public class News
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime PostTime { get; set; }
        public bool IsActive { get; set; }
        public Guid UserId { get; set; }
    }
}
