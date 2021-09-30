using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;

namespace LT.DigitalOffice.NewsService.Mappers.Responses
{
  public class NewsResponseMapper : INewsResponseMapper
  {
    public NewsResponse Map(DbNews dbNews, DepartmentInfo department, UserInfo author)
    {
      if (dbNews == null)
      {
        return null;
      }

      return new NewsResponse
      {
        Id = dbNews.Id,
        Preview = dbNews.Preview,
        Content = dbNews.Content,
        Subject = dbNews.Subject,
        Pseudonym = dbNews.Pseudonym,
        Author = author,
        Department = department,
        IsActive = dbNews.IsActive,
        CreatedAtUtc = dbNews.CreatedAtUtc,
        CreatedBy = dbNews.CreatedBy
      };
    }
  }
}
