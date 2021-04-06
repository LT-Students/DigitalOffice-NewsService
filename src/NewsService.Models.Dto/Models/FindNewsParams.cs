using System;

namespace LT.DigitalOffice.NewsService.Models.Dto.Models
{
    public class FindNewsParams
    {
        public Guid? AuthorId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string Pseudonym { get; set; }
        public string Subject { get; set; }
    }
}
