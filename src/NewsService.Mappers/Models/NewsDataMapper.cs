using LT.DigitalOffice.Models.Broker.Models.News;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class NewsDataMapper : INewsDataMapper
  {
    public NewsData Map(DbNews dbNews)
    {
      if (dbNews == null)
      {
        return null;
      }

      return new NewsData
      {
        Id = dbNews.Id,
        Preview = dbNews.Preview,
        Subject = dbNews.Subject,
        Pseudonym = dbNews.Pseudonym,
        AuthorId = dbNews.AuthorId,
        CreatedAtUtc = dbNews.CreatedAtUtc,
        SenderId = dbNews.CreatedBy
      };
    }
  }
}
