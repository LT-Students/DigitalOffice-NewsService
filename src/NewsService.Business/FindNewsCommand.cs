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

    private async Task<List<ImageData>> GetImages(List<Guid> imagesIds, List<string> errors)
    {
      if (imagesIds == null || imagesIds.Count == 0)
      {
        return null;
      }

      string errorMessage = "Cannot get avatar Images now. Please try again later.";
      string logMessage = "Cannot get avatar images with ids: {imagesId}.";

      try
      {
        Response<IOperationResult<IGetImagesResponse>> response = await _rcGetImages
          .GetResponse<IOperationResult<IGetImagesResponse>>(IGetImagesRequest.CreateObj(imagesIds, ImageSource.News));

        if (response.Message.IsSuccess)
        {
          return response.Message.Body.ImagesData;
        }

        _logger.LogWarning(logMessage, string.Join(", ", imagesIds));
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, logMessage, string.Join(", ", imagesIds));
      }

      errors.Add(errorMessage);

      return null;
    }

    private async Task<List<UserData>> GetAuthors(List<Guid> authorIds, List<string> errors)
    {
      if (authorIds == null || !authorIds.Any())
      {
        return null;
      }

      string errorMessage = "Cannot get authors now. Please try again later.";
      string logMessage = "Cannot get authors with ids: {authorIds}.";

      try
      {
        Response<IOperationResult<IGetUsersDataResponse>> response = await _rcGetUsers
          .GetResponse<IOperationResult<IGetUsersDataResponse>>(IGetUsersDataRequest.CreateObj(authorIds));

        if (response.Message.IsSuccess)
        {
          return response.Message.Body.UsersData;
        }

        _logger.LogWarning(logMessage, string.Join(", ", authorIds));
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, logMessage, string.Join(", ", authorIds));
      }

      errors.Add(errorMessage);

      return null;
    }

    private async Task<List<DepartmentData>> GetDepartments(List<Guid> departmentIds, List<string> errors)
    {
      if (departmentIds == null || departmentIds.Count == 0)
      {
        return null;
      }

      string errorMessage = "Can not get departments. Please try again later.";

      try
      {
        Response<IOperationResult<IGetDepartmentsResponse>> response = await _rcGetDepartments
          .GetResponse<IOperationResult<IGetDepartmentsResponse>>(IGetDepartmentsRequest.CreateObj(departmentIds));

        if (response.Message.IsSuccess)
        {
          return response.Message.Body.Departments;
        }
        else
        {
          _logger.LogWarning("Errors while getting departments. Reason: {Errors}",
            string.Join('\n', response.Message.Errors));
        }
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, errorMessage);
      }

      errors.Add(errorMessage);

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

      List<DepartmentData> departments = await GetDepartments(
        dbNewsList.Where(d => d.DepartmentId.HasValue).Select(d => d.DepartmentId.Value).Distinct().ToList(),
        response.Errors);
      List<DepartmentInfo> departmentsInfo = departments.Select(_departmentInfoMapper.Map).ToList();

      List<UserData> authors = await GetAuthors(
        dbNewsList.Select(a => a.AuthorId).Distinct().ToList(),
        response.Errors);
      List<Guid> imagesIds = authors?.Where(u => u.ImageId.HasValue).Select(u => u.ImageId.Value).ToList();
      List<ImageData> avatarImages = await GetImages(imagesIds, response.Errors);
      List<UserInfo> authorsInfo = authors.Select(a => _userInfoMapper.Map(a, avatarImages?.FirstOrDefault(i => a.ImageId == i.ImageId))).ToList();

      response.Body = dbNewsList
        .Select(dbNews => _mapper.Map(
          dbNews,
          departmentsInfo.FirstOrDefault(d => dbNews.DepartmentId == d.Id),
          authorsInfo.FirstOrDefault(a => dbNews.AuthorId == a.Id)))
        .ToList();
      response.TotalCount = totalCount;
      response.Status = response.Errors.Any()
        ? OperationResultStatusType.PartialSuccess
        : OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
