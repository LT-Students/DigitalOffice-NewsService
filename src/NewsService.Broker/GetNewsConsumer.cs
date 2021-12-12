using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Models.Broker.Models.News;
using LT.DigitalOffice.Models.Broker.Requests.News;
using LT.DigitalOffice.Models.Broker.Responses.News;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using MassTransit;

namespace LT.DigitalOffice.NewsService.Broker
{
  public class GetNewsConsumer : IConsumer<IGetNewsRequest>
  {
    private readonly INewsRepository _repository;
    private readonly INewsDataMapper _mapper;

    public GetNewsConsumer (
      INewsRepository repository,
      INewsDataMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }
    private async Task<object> GetNewsAsync(IGetNewsRequest request)
    {
      List<NewsData> News = new();
      foreach(var obj in request.NewsIds)
      {
        DbNews dbNews = await _repository.GetAsync(obj);
        News.Add(_mapper.Map(dbNews));      
      }     
      return IGetNewsResponse.CreateObj(News);
    }
    
    public async Task Consume(ConsumeContext<IGetNewsRequest> context)
    {
      object response = OperationResultWrapper.CreateResponse(GetNewsAsync, context.Message);

      await context.RespondAsync<IOperationResult<IGetNewsResponse>>(response);
    }
  }
}
