using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Models.Broker.Models.News;
using LT.DigitalOffice.Models.Broker.Requests.News;
using LT.DigitalOffice.Models.Broker.Responses.News;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using MassTransit;

namespace LT.DigitalOffice.NewsService.Broker
{
  public class GetNewsConsumer : IConsumer<IGetNewsRequest>
  {
    private readonly INewsRepository _repository;

    public GetNewsConsumer(
      INewsRepository repository)
    {
      _repository = repository;
    }

    private async Task<List<NewsData>> GetNewsAsync(IGetNewsRequest request)
    {
      List<DbNews> News = await _repository.GetAsync(request.NewsIds);

      return News
        .Select(
        n => new NewsData(
          n.Id,
          n.Preview,
          n.Subject,
          n.Pseudonym,
          n.AuthorId,
          n.CreatedAtUtc,
          n.CreatedBy))
        .ToList();
    }

    public async Task Consume(ConsumeContext<IGetNewsRequest> context)
    {
      List<NewsData> news = await GetNewsAsync(context.Message);

      await context.RespondAsync<IOperationResult<IGetNewsResponse>>(
        OperationResultWrapper.CreateResponse((_) => IGetNewsResponse.CreateObj(news), context));
    }
  }
}
