using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Business.Commands.Tags.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Tag;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class TagController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<Guid?>> CreateAsync(
      [FromServices] ICreateTagCommand command,
      [FromBody] CreateTagRequest request)
    {
      return await command.ExecuteAsync(request);
    }

    [HttpGet("find")]
    public async Task<FindResultResponse<TagInfo>> FindAsync(
      [FromServices] IFindTagCommand command,
      [FromQuery] FindTagFilter filter)
    {
      return await command.ExecuteAsync(filter);
    }

    [HttpPut("edit")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditTagCommand command,
      [FromQuery] Guid newsId,
      [FromBody] EditTagsRequest request)
    {
      return await command.ExecuteAsync(newsId, request);
    }
  }
}
