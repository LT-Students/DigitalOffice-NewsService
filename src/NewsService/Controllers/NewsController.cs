using System;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Business.Commands.News.Create;
using LT.DigitalOffice.NewsService.Business.Commands.News.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class NewsController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IAccessValidator _accessValidator;

    public NewsController(
      IMediator mediator,
      IAccessValidator accessValidator)
    {
      _mediator = mediator;
      _accessValidator = accessValidator;
    }

    [HttpGet("get")]
    public async Task<OperationResultResponse<NewsResponse>> GetAsync(
      [FromServices] IGetNewsCommand command,
      [FromQuery] Guid newsId)
    {
      return await command.ExecuteAsync(newsId);
    }

    [HttpPatch("edit")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditNewsCommand command,
      [FromQuery] Guid newsId,
      [FromBody] JsonPatchDocument<EditNewsRequest> request)
    {
      return await command.ExecuteAsync(newsId, request);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(
      [FromBody] CreateNewsRequest request,
      CancellationToken token)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveNews))
      {
        return StatusCode(403);
      }

      return Created("/news", await _mediator.Send(request, token));
    }

    [HttpGet("find")]
    public async Task<FindResultResponse<NewsInfo>> FindAsync(
      [FromServices] IFindNewsCommand command,
      [FromQuery] FindNewsFilter findNewsFilter)
    {
      return await command.ExecuteAsync(findNewsFilter);
    }
  }
}
