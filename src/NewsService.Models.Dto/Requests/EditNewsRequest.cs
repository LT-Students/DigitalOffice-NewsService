using System;
using System.Collections.Generic;
using System.Linq;
namespace LT.DigitalOffice.NewsService.Models.Dto.Requests
{
    public class EditNewsRequest
    {
        public string Content { get; set; }
        public string Subject { get; set; }
        public bool IsActive { get; set; }
    }
}
