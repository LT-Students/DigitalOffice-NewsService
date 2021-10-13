using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Models.Company;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Requests.Image;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using LT.DigitalOffice.Models.Broker.Responses.Image;
using LT.DigitalOffice.Models.Broker.Responses.User;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.NewsService.Business
{
  public class GetNewsCommand : IGetNewsCommand
  {
    private readonly INewsRepository _repository;
    private readonly INewsResponseMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IRequestClient<IGetDepartmentsRequest> _rcGetDepartments;
    private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
    private readonly IDepartmentInfoMapper _departmentInfoMapper;
    private readonly IUserInfoMapper _userInfoMapper;
    private readonly ILogger<GetNewsCommand> _logger;

    private async Task<List<UserData>> GetUsersDataAsync(List<Guid> usersIds, List<string> errors)
    {
      if (usersIds == null || !usersIds.Any())
      {
        return null;
      }

      try
      {
        Response<IOperationResult<IGetUsersDataResponse>> response =
          await _rcGetUsers.GetResponse<IOperationResult<IGetUsersDataResponse>>(
            IGetUsersDataRequest.CreateObj(usersIds.Distinct().ToList()));

        if (response.Message.IsSuccess)
        {
          return response.Message.Body.UsersData;
        }

        _logger.LogWarning(
          "Error while geting users data by ids: {UsersIds}. Reason:{Errors}",
          string.Join(", ", usersIds),
          string.Join('\n', response.Message.Errors));
      }
      catch (Exception exc)
      {
        _logger.LogError(
          "Can not get users data by ids: {UsersIds}. {ErrorsMessage}",
          string.Join(", ", usersIds),
          exc.Message);
      }

      errors.Add("Can not get users data. Please try again later.");

      return null;
    }

    private async Task<List<DepartmentData>> GetDepartmentAsync(Guid? departmentId, List<string> errors)
    {
      if (departmentId == null)
      {
        return null;
      }

      try
      {
        Response<IOperationResult<IGetDepartmentsResponse>> response =
          await _rcGetDepartments.GetResponse<IOperationResult<IGetDepartmentsResponse>>(
            IGetDepartmentsRequest.CreateObj(new List<Guid> { departmentId.Value }));

        if (response.Message.IsSuccess)
        {
          return response.Message.Body.Departments;
        }

        _logger.LogWarning(
          "Error while getting department id {DepartmentId}. Reason:{Errors}",
          departmentId,
          string.Join('\n', response.Message.Errors));
      }
      catch (Exception exc)
      {
        _logger.LogError("Can not get department id {DepartmentId}. {ErrorsMessage}", exc.Message);
      }

      errors.Add("Can not get department info. Please try again later.");

      return null;
    }

    private async Task<List<ImageData>> GetUsersAvatarsAsync(List<Guid> imagesIds, List<string> errors)
    {
      if (imagesIds == null || !imagesIds.Any())
      {
        return null;
      }

      try
      {
        Response<IOperationResult<IGetImagesResponse>> response =
          await _rcGetImages.GetResponse<IOperationResult<IGetImagesResponse>>(
            IGetImagesRequest.CreateObj(imagesIds, ImageSource.News));

        if (response.Message.IsSuccess)
        {
          return response.Message.Body.ImagesData;
        }

        _logger.LogWarning(
          "Error while getting images by ids: {ImagesIds}. Reason:{Errors}",
          string.Join(", ", imagesIds),
          string.Join('\n', response.Message.Errors));
      }
      catch (Exception exc)
      {
        _logger.LogError("Can not get images by ids: {ImagesIds}. {ErrorsMessage}", imagesIds, exc.Message);
      }

      errors.Add("Can not get images. Please try again later.");

      return null;
    }

    public GetNewsCommand(
      INewsRepository repository,
      INewsResponseMapper mapper,
      IHttpContextAccessor httpContextAccessor,
      IRequestClient<IGetDepartmentsRequest> rcGetDepartments,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      IRequestClient<IGetImagesRequest> rcGetImages,
      IDepartmentInfoMapper departmentMapper,
      IUserInfoMapper userInfoMapper,
      ILogger<GetNewsCommand> logger)
    {
      _repository = repository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
      _rcGetDepartments = rcGetDepartments;
      _rcGetUsers = rcGetUsers;
      _rcGetImages = rcGetImages;
      _departmentInfoMapper = departmentMapper;
      _userInfoMapper = userInfoMapper;
      _logger = logger;
    }

    public async Task<OperationResultResponse<NewsResponse>> ExecuteAsync(Guid newsId)
    {
      OperationResultResponse<NewsResponse> response = new();

      DbNews dbNews = await _repository.GetAsync(newsId);
      if (dbNews == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

        response.Status = OperationResultStatusType.Failed;
        return response;
      }

      List<DepartmentData> departmentsData = await GetDepartmentAsync(dbNews.DepartmentId, response.Errors);

      List<UserData> usersData =
        await GetUsersDataAsync(
          new List<Guid>() { dbNews.AuthorId, dbNews.CreatedBy },
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
          _departmentInfoMapper.Map(departmentsData?.FirstOrDefault()),
          usersInfo?.FirstOrDefault(ui => ui.Id == dbNews.AuthorId),
          usersInfo?.FirstOrDefault(ui => ui.Id == dbNews.CreatedBy));

      response.Status = response.Errors.Any()
        ? OperationResultStatusType.PartialSuccess
        : OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
