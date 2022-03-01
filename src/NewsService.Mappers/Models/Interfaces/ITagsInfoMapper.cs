using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface ITagsInfoMapper
  {
    public List<TagsInfo> Map(List<DbTags> dbTags);
  }
}
