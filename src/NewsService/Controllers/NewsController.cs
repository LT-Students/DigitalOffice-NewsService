using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class NewsController : ControllerBase
  {
    [HttpGet("get")]
    public OperationResultResponse<NewsResponse> Get(
      [FromServices] IGetNewsCommand command,
      [FromQuery] Guid newsId)
    {
      return command.Execute(newsId);
    }

    [HttpPatch("edit")]
    public OperationResultResponse<bool> Edit(
      [FromServices] IEditNewsCommand command,
      [FromQuery] Guid newsId,
      [FromBody] JsonPatchDocument<EditNewsRequest> request)
    {
      return command.Execute(newsId, request);
    }

    [HttpPost("create")]
    public async Task<OperationResultResponse<Guid?>> Create(
      [FromServices] ICreateNewsCommand command,
      [FromBody] CreateNewsRequest request)
    {
      return await command.Execute(request);
    }

    [HttpGet("find")]
    public async Task<FindResultResponse<NewsInfo>> Find(
      [FromServices] IFindNewsCommand command,
      [FromQuery] FindNewsFilter findNewsFilter)
    {
      return await command.Execute(findNewsFilter);
    }
  }
}
