using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Business.Commands.Channels.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ChannelController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<Guid>> Create(
      [FromServices] ICreateChannelCommand command,
      [FromBody] CreateChannelRequest request)
    {
      return await command.ExecuteAsync(request);
    }

    [HttpGet("get")]
    public async Task<OperationResultResponse<ChannelResponse>> Get(
      [FromServices] IGetChannelCommand command,
      [FromQuery] Guid channelId)
    {
      return await command.ExecuteAsync(channelId);
    }

    [HttpGet("find")]
    public async Task<FindResultResponse<ChannelInfo>> Find(
      [FromServices] IFindChannelCommand command,
      [FromQuery] FindChannelFilter findNewsFilter)
    {
      return await command.ExecuteAsync(findNewsFilter);
    }

    [HttpPatch("edit")]
    public async Task<OperationResultResponse<bool>> Edit(
      [FromServices] IEditChannelCommand command,
      [FromQuery] Guid channelId,
      [FromBody] JsonPatchDocument<EditChannelRequest> request)
    {
      return await command.ExecuteAsync(channelId, request);
    }
  }
}
