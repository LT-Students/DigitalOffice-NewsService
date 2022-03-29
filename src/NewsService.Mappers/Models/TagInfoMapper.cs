using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class TagInfoMapper : ITagInfoMapper
  {
    public List<TagInfo> Map(List<DbTag> dbTagsList)
    {
      if (dbTagsList is null)
      {
        return null;
      }

      return dbTagsList.Select(x => new TagInfo
      {
        Id = x.Id,
        Name = x.Name,
        Count = x.Count,
      }).ToList();
    }
  }
}
