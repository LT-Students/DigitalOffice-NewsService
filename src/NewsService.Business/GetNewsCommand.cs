using System;
using System.Net;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.NewsService.Business
{
  public class GetNewsCommand : IGetNewsCommand
  {
    private readonly INewsRepository _repository;
    private readonly INewsResponseMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetNewsCommand(
      INewsRepository repository,
      INewsResponseMapper mapper,
      IHttpContextAccessor httpContextAccessor)
    {
      _repository = repository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public OperationResultResponse<NewsResponse> Execute(Guid newsId)
    {
      OperationResultResponse<NewsResponse> response = new();

      response.Body = _mapper.Map(_repository.Get(newsId));
      if (response.Body == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

        response.Status = OperationResultStatusType.Failed;
      }
      response.Status = OperationResultStatusType.FullSuccess;
      return response;
    }
  }
}
