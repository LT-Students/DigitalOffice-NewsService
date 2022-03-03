using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.Image;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.Image;
using LT.DigitalOffice.Models.Broker.Responses.User;
using LT.DigitalOffice.NewsService.Business.Commands.News.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.NewsService.Business.Commands.News
{
  public class FindNewsCommand : IFindNewsCommand
  {
    private readonly INewsRepository _repository;
    private readonly INewsInfoMapper _mapper;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
    private readonly ILogger<GetNewsCommand> _logger;
    private readonly IBaseFindFilterValidator _baseFindValidator;
    private readonly IUserInfoMapper _userInfoMapper;
    private readonly IResponseCreator _responseCreator;
    private readonly IChannelInfoMapper _channelInfoMapper;

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

    public FindNewsCommand(
      INewsRepository repository,
      INewsInfoMapper mapper,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      IRequestClient<IGetImagesRequest> rcGetImages,
      IUserInfoMapper userInfoMapper,
      IResponseCreator responseCreator,
      IChannelInfoMapper channelInfoMapper,
      ILogger<GetNewsCommand> logger,
      IBaseFindFilterValidator baseFindValidator)
    {
      _repository = repository;
      _mapper = mapper;
      _rcGetUsers = rcGetUsers;
      _rcGetImages = rcGetImages;
      _logger = logger;
      _baseFindValidator = baseFindValidator;
      _userInfoMapper = userInfoMapper;
      _responseCreator = responseCreator;
      _channelInfoMapper = channelInfoMapper;
    }
    public async Task<FindResultResponse<NewsInfo>> ExecuteAsync(FindNewsFilter findNewsFilter)
    {
      if (!_baseFindValidator.ValidateCustom(findNewsFilter, out List<string> errors))
      {
        return _responseCreator.CreateFailureFindResponse<NewsInfo>(
          HttpStatusCode.BadRequest);
      }

      (List<DbNews> dbNewsList, int totalCount) = await _repository.FindAsync(findNewsFilter);

      if (dbNewsList is null)
      {
        return _responseCreator.CreateFailureFindResponse<NewsInfo>(
          HttpStatusCode.NotFound);
      }

      FindResultResponse<NewsInfo> response = new();

      List<UserData> usersData = await GetUsersDataAsync(
        dbNewsList
          .Where(n => n.PublishedBy.HasValue)
          .Select(n => n.PublishedBy.Value)
          .Concat(dbNewsList.Select(n => n.CreatedBy))
          .Distinct()
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

      response.Body = dbNewsList
        .Select(dbNews => _mapper.Map(
          dbNews,
          usersInfo,
          _channelInfoMapper.Map(dbNews.Channel)))
        .ToList();

      response.TotalCount = totalCount;

      response.Status = response.Errors.Any()
        ? OperationResultStatusType.PartialSuccess
        : OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
