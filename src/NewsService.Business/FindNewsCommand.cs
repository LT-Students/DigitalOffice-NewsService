using System.Linq;
using System.Net;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.NewsService.Business
{
  public class FindNewsCommand : IFindNewsCommand
  {
    private readonly INewsRepository _repository;
    private readonly INewsInfoMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FindNewsCommand(
      INewsRepository repository,
      INewsInfoMapper mapper,
      IHttpContextAccessor httpContextAccessor)
    {
      _repository = repository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public FindResultResponse<NewsInfo> Execute(FindNewsFilter findNewsFilter)
    {
      FindResultResponse<NewsInfo> response = new();

      response.Body = _repository
        .Find(findNewsFilter, out int totalCount)
        .Select(_mapper.Map)
        .ToList();

      response.TotalCount = totalCount;

      if (response.Errors.Any())
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
        return response;
      }

      response.Status = OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
