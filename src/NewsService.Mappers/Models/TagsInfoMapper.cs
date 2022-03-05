﻿using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class TagsInfoMapper : ITagsInfoMapper
  {
    public List<TagsInfo> Map(List<DbTag> dbTags)
    {
      if (dbTags is null)
      {
        return null;
      }

      return dbTags.Select(x => new TagsInfo
      {
        Id = x.Id,
        Name = x.Name,
        Count = x.Count,
      }).ToList();
    }
  }
}
