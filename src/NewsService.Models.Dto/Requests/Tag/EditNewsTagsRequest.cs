using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests.Tag
{
  public record EditNewsTagsRequest
  {
    public List<Guid> TagsToAdd { get; set; }
    public List<Guid> TagsToRemove { get; set; }
  }
}
