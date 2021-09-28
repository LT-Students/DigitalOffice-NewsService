using LT.DigitalOffice.Models.Broker.Models.Company;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class DepartmentInfoMapper : IDepartmentInfoMapper
  {
    public DepartmentInfo Map(DepartmentData department)
    {
      if (department == null)
      {
        return null;
      }

      return new DepartmentInfo
      {
        Id = department.Id,
        Name = department.Name
      };
    }
  }
}
