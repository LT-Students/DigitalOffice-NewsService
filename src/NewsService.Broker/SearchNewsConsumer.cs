using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.News;
using LT.DigitalOffice.Models.Broker.Responses.Search;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using MassTransit;

namespace LT.DigitalOffice.NewsService.Broker
{
  public class SearchNewsConsumer : IConsumer<ISearchNewsRequest>
  {
    private readonly INewsRepository _repository;

    public SearchNewsConsumer(INewsRepository repository)
    {
      _repository = repository;
    }

    private async Task<object> SearchNewsAsync(string text)
    {
      List<DbNews> news = await _repository.SearchAsync(text);

      return ISearchResponse.CreateObj(
        news.Select(
          n => new SearchInfo(n.Id, n.Subject)).ToList());
    }

    public async Task Consume(ConsumeContext<ISearchNewsRequest> context)
    {
      object response = OperationResultWrapper.CreateResponse(SearchNewsAsync, context.Message.Value);

      await context.RespondAsync<IOperationResult<ISearchResponse>>(response);
    }
  }
}
