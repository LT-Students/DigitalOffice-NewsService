using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.Image;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.Image;
using LT.DigitalOffice.Models.Broker.Responses.User;
using LT.DigitalOffice.NewsService.Business.Commands.Channels.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.NewsService.Business.Commands.Channels
{
  public class GetChannelCommand : IGetChannelCommand
  {
    private readonly IChannelRepository _channelRepository;
    private readonly IResponseCreator _responseCreator;
    private readonly IChannelResponseMapper _channelResponseMapper;
    private readonly INewsInfoMapper _newsInfoMapper;
    private readonly ILogger<GetChannelCommand> _logger;
    private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IUserInfoMapper _userInfoMapper;
    private readonly IChannelInfoMapper _channelInfoMapper;
    private readonly ITagsRepository _tagsRepository;
    private readonly ITagsInfoMapper _tagsInfoMapper;

    private async Task<List<ImageData>> GetImagesDataAsync(List<Guid> imagesIds, List<string> errors)
    {
      if (imagesIds == null || !imagesIds.Any())
      {
        return null;
      }

      return (await RequestHandler.ProcessRequest<IGetImagesRequest, IGetImagesResponse>(
        _rcGetImages,
        IGetImagesRequest.CreateObj(imagesIds, ImageSource.User),
        errors,
        _logger))
      .ImagesData;
    }

    private async Task<List<UserData>> GetUsersDataAsync(List<Guid> usersIds, List<string> errors)
    {
      if (usersIds is null || !usersIds.Any())
      {
        return null;
      }

      return (await RequestHandler.ProcessRequest<IGetUsersDataRequest, IGetUsersDataResponse>(
        _rcGetUsers,
        IGetUsersDataRequest.CreateObj(usersIds),
        errors,
        _logger))
      .UsersData;
    }

    public GetChannelCommand(
      IResponseCreator responseCreator,
      IChannelRepository channelRepository,
      IChannelResponseMapper channelResponseMapper,
      INewsInfoMapper newsInfoMapper,
      IRequestClient<IGetImagesRequest> rcGetImages,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      IUserInfoMapper userInfoMapper,
      ITagsRepository tagsRepository,
      IChannelInfoMapper channelInfoMapper,
      ITagsInfoMapper tagsInfoMapper,
      ILogger<GetChannelCommand> logger)
    {
      _responseCreator = responseCreator;
      _channelRepository = channelRepository;
      _channelResponseMapper = channelResponseMapper;
      _newsInfoMapper = newsInfoMapper;
      _rcGetImages = rcGetImages;
      _rcGetUsers = rcGetUsers;
      _logger = logger;
      _userInfoMapper = userInfoMapper;
      _channelRepository = channelRepository;
      _tagsRepository = tagsRepository;
      _tagsInfoMapper = tagsInfoMapper;
      _channelInfoMapper = channelInfoMapper;
    }

    public async Task<OperationResultResponse<ChannelResponse>> ExecuteAsync(Guid channelId)
    {
      OperationResultResponse<ChannelResponse> response = new();

      DbChannel dbChannel = await _channelRepository.GetAsync(channelId);
      if (dbChannel is null)
      {
        return _responseCreator.CreateFailureResponse<ChannelResponse>(
          HttpStatusCode.NotFound);
      }

      List<UserData> usersData = await GetUsersDataAsync(
        dbChannel.News
          .Where(c => c.PublishedBy.HasValue)
          .Select(c => c.PublishedBy.Value)
          .Concat(dbChannel.News.Select(c => c.CreatedBy))
          .ToList(),
        response.Errors);

      List<ImageData> avatarImages = await GetImagesDataAsync(
        usersData?
          .Where(u => u.ImageId.HasValue)
          .Select(u => u.ImageId.Value)
          .ToList(),
        response.Errors);

      List<UserInfo> usersInfo = usersData?
        .Select(ud => _userInfoMapper.Map(ud, avatarImages?.FirstOrDefault(id => ud.ImageId == id.ImageId)))
        .ToList();

      response.Body = _channelResponseMapper
        .Map(
          dbChannel,
          _newsInfoMapper.Map(
            dbChannel.News.ToList(),
            usersInfo,
           _channelInfoMapper.Map(dbChannel)));

      response.Status = response.Errors.Any()
        ? OperationResultStatusType.PartialSuccess
        : OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
