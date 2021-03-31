using System;

namespace LT.DigitalOffice.NewsService.Models.Dto.Models
{
    public class FindNewsParams
    {
        public Guid? AuthorId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string AuthorPseudonym { get; set; }
        public string NewsName { get; set; }
    }
}
