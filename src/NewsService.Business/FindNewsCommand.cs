using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
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
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.NewsService.Business
{
  public class FindNewsCommand : IFindNewsCommand
  {
    private readonly INewsRepository _repository;
    private readonly INewsInfoMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IRequestClient<IGetDepartmentsRequest> _rcGetDepartments;
    private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
    private readonly ILogger<GetNewsCommand> _logger;
    private readonly IBaseFindRequestValidator _baseFindValidator;
    private readonly IDepartmentInfoMapper _departmentInfoMapper;
    private readonly IUserInfoMapper _userInfoMapper;

    private async Task<List<ImageData>> GetImagesData(List<Guid> imagesIds, List<string> errors)
    {
      if (imagesIds == null || !imagesIds.Any())
      {
        return null;
      }

      try
      {
        Response<IOperationResult<IGetImagesResponse>> response =
          await _rcGetImages.GetResponse<IOperationResult<IGetImagesResponse>>(
            IGetImagesRequest.CreateObj(imagesIds.Distinct().ToList(), ImageSource.News));

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
        _logger.LogError(
          "Can not get images by ids: {ImagesIds}. {ErrorsMessage}",
          string.Join(", ", imagesIds),
          exc.Message);
      }

      errors.Add("Can not get images data.Please try again later.");

      return null;
    }

    private async Task<List<UserData>> GetUsersData(List<Guid> usersIds, List<string> errors)
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
          "Error while getting users data by ids: {UsersIds}. Reason:{Errors}",
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

    private async Task<List<DepartmentData>> GetDepartments(List<Guid> departmentsIds, List<string> errors)
    {
      if (departmentsIds == null || !departmentsIds.Any())
      {
        return null;
      }

      try
      {
        Response<IOperationResult<IGetDepartmentsResponse>> response =
          await _rcGetDepartments.GetResponse<IOperationResult<IGetDepartmentsResponse>>(
            IGetDepartmentsRequest.CreateObj(departmentsIds));

        if (response.Message.IsSuccess)
        {
          return response.Message.Body.Departments;
        }
        else
        {
          _logger.LogWarning(
            "Error while getting departments by ids: {DepartmentsIds}. Reason:{errors}",
          string.Join(", ", departmentsIds),
          string.Join('\n', response.Message.Errors));
        }
      }
      catch (Exception exc)
      {
        _logger.LogError(
          "Can not get departments by ids: {DepartmentsIds}. {ErrorsMessage}",
          string.Join(", ", departmentsIds),
          exc.Message);
      }

      errors.Add("Can not get departments data. Please try again later.");

      return null;
    }

    public FindNewsCommand(
      INewsRepository repository,
      INewsInfoMapper mapper,
      IHttpContextAccessor httpContextAccessor,
      IRequestClient<IGetDepartmentsRequest> rcGetDepartments,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      IRequestClient<IGetImagesRequest> rcGetImages,
      IDepartmentInfoMapper departmentInfoMapper,
      IUserInfoMapper userInfoMapper,
      ILogger<GetNewsCommand> logger,
      IBaseFindRequestValidator baseFindValidator)
    {
      _repository = repository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
      _rcGetDepartments = rcGetDepartments;
      _rcGetUsers = rcGetUsers;
      _rcGetImages = rcGetImages;
      _logger = logger;
      _baseFindValidator = baseFindValidator;
      _departmentInfoMapper = departmentInfoMapper;
      _userInfoMapper = userInfoMapper;
    }

    public async Task<FindResultResponse<NewsInfo>> Execute(FindNewsFilter findNewsFilter)
    {
      FindResultResponse<NewsInfo> response = new();

      if (!_baseFindValidator.ValidateCustom(findNewsFilter, out List<string> errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
        response.Errors = errors;
        return response;
      }

      List<DbNews> dbNewsList = _repository.Find(findNewsFilter, out int totalCount);

      if (dbNewsList == null)
      {
        return response;
      }

      List<DepartmentData> departmentsData = await GetDepartments(
        dbNewsList.Where(d => d.DepartmentId.HasValue).Select(d => d.DepartmentId.Value).ToList(),
        response.Errors);

      List<DepartmentInfo> departmentsInfo = departmentsData?.Select(_departmentInfoMapper.Map).ToList();

      List<UserData> usersData = await GetUsersData(
        dbNewsList.Select(n => n.AuthorId).Concat(dbNewsList.Select(n => n.CreatedBy)).ToList(),
        response.Errors);

      List<ImageData> avatarImages = await GetImagesData(
        usersData?.Where(u => u.ImageId.HasValue).Select(u => u.ImageId.Value).ToList(),
        response.Errors);

      List<UserInfo> usersInfo = usersData?
        .Select(ud => _userInfoMapper.Map(ud, avatarImages?.FirstOrDefault(id => ud.ImageId == id.ImageId))).ToList();

      response.Body = dbNewsList
        .Select(dbNews => _mapper.Map(
          dbNews,
          departmentsInfo?.FirstOrDefault(di => dbNews.DepartmentId == di.Id),
          usersInfo?.FirstOrDefault(ud => dbNews.AuthorId == ud.Id),
          usersInfo?.FirstOrDefault(ud => dbNews.CreatedBy == ud.Id)))
        .ToList();

      response.TotalCount = totalCount;

      response.Status = response.Errors.Any()
        ? OperationResultStatusType.PartialSuccess
        : OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
