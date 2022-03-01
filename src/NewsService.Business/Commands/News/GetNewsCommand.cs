﻿using System;
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
using LT.DigitalOffice.NewsService.Business.Commands.News.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.NewsService.Business.Commands.News
{
  public class GetNewsCommand : IGetNewsCommand
  {
    private readonly INewsRepository _newsRepository;
    private readonly INewsResponseMapper _mapper;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
    private readonly IUserInfoMapper _userInfoMapper;
    private readonly IResponseCreator _responseCreator;
    private readonly ITagsInfoMapper _tagsInfoMapper;
    private readonly IChannelRepository _channelRepository;
    private readonly ITagsRepository _tagsRepository;
    private readonly IChannelInfoMapper _channelInfoMapper;
    private readonly ILogger<GetNewsCommand> _logger;

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

    private async Task<List<ImageData>> GetUsersAvatarsAsync(List<Guid> imagesIds, List<string> errors)
    {
      if (imagesIds is null || !imagesIds.Any())
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

    public GetNewsCommand(
      INewsRepository newsRepository,
      INewsResponseMapper mapper,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      IRequestClient<IGetImagesRequest> rcGetImages,
      IUserInfoMapper userInfoMapper,
      IResponseCreator responseCreator,
      ITagsInfoMapper tagsInfoMapper,
      IChannelRepository channelRepository,
      ITagsRepository tagsRepository,
      IChannelInfoMapper channelInfoMapper,
      ILogger<GetNewsCommand> logger)
    {
      _newsRepository = newsRepository;
      _mapper = mapper;
      _rcGetUsers = rcGetUsers;
      _rcGetImages = rcGetImages;
      _userInfoMapper = userInfoMapper;
      _responseCreator = responseCreator;
      _tagsInfoMapper = tagsInfoMapper;
      _channelRepository = channelRepository;
      _tagsRepository = tagsRepository;
      _channelInfoMapper = channelInfoMapper;
      _logger = logger;
    }

    public async Task<OperationResultResponse<NewsResponse>> ExecuteAsync(Guid newsId)
    {
      OperationResultResponse<NewsResponse> response = new();

      DbNews dbNews = await _newsRepository.GetAsync(newsId);
      if (dbNews is null)
      {
        return _responseCreator.CreateFailureResponse<NewsResponse>(
          HttpStatusCode.NotFound);
      }

      List<UserData> usersData =
        await GetUsersDataAsync(
          new List<Guid>() {dbNews.PublishedBy.Value, dbNews.CreatedBy},
          response.Errors);

      List<ImageData> avatarsImages =
        await GetUsersAvatarsAsync(
          usersData?.Where(ud => ud.ImageId.HasValue).Select(ud => ud.ImageId.Value).ToList(),
          response.Errors);

      List<UserInfo> usersInfo =
        usersData?
          .Select(ud => _userInfoMapper.Map(ud, avatarsImages?.FirstOrDefault(ai => ai.ImageId == ud.ImageId))).ToList();

      response.Body = _mapper
        .Map(
          dbNews,
          usersInfo?.FirstOrDefault(ui => ui.Id == dbNews.PublishedBy),
          usersInfo?.FirstOrDefault(ui => ui.Id == dbNews.CreatedBy),
          _channelInfoMapper.Map(dbNews.Channel),
          _tagsInfoMapper.Map(null));///////////

      response.Status = response.Errors.Any()
        ? OperationResultStatusType.PartialSuccess
        : OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
