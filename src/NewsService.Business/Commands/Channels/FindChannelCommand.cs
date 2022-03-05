using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.NewsService.Business.Commands.Channels.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;

namespace LT.DigitalOffice.NewsService.Business.Commands.Channels
{
  public class FindChannelCommand : IFindChannelCommand
  {
    private readonly IBaseFindFilterValidator _baseFindValidator;
    private readonly IResponseCreator _responseCreator;
    private readonly IChannelRepository _channelRepository;
    private readonly IChannelInfoMapper _channelInfoMapper;

    public FindChannelCommand(
      IBaseFindFilterValidator baseFindValidator,
      IResponseCreator responseCreator,
      IChannelRepository channelRepository,
      IChannelInfoMapper channelInfoMapper)
    {
      _responseCreator = responseCreator;
      _baseFindValidator = baseFindValidator;
      _channelRepository = channelRepository;
      _channelInfoMapper = channelInfoMapper;
    }

    public async Task<FindResultResponse<ChannelInfo>> ExecuteAsync(FindChannelFilter filter)
    {
      if (!_baseFindValidator.ValidateCustom(filter, out List<string> errors))
      {
        return _responseCreator.CreateFailureFindResponse<ChannelInfo>(HttpStatusCode.BadRequest, errors);
      }

      (List<DbChannel> dbChannels, int totalCount) = await _channelRepository.FindAsync(filter);

      FindResultResponse<ChannelInfo> response = new();

      response.Body = dbChannels.Select(dbChannel => _channelInfoMapper.Map(dbChannel)).ToList();

      response.TotalCount = totalCount;

      return response;
    }
  }
}
