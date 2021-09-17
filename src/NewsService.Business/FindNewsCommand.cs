using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Models.Company;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.Company;
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
    private readonly IRequestClient<IGetUsersDataRequest> _usersRequestClient;
    private readonly IRequestClient<IGetDepartmentsRequest> _departmentsRequestClient;
    private readonly ILogger<GetNewsCommand> _logger;

    private List<UserData> GetAuthor(List<Guid> authorIds, List<string> errors)
    {
      if (authorIds == null || authorIds.Count == 0)
      {
        return null;
      }

      string errorMessage = "Cannot get authors now. Please try again later.";
      const string logMessage = "Cannot get authors with ids: {authorIds}.";

      try
      {
        object request = IGetUsersDataRequest.CreateObj(authorIds);
        Response<IOperationResult<IGetUsersDataResponse>> response = _usersRequestClient
          .GetResponse<IOperationResult<IGetUsersDataResponse>>(request)
          .Result;

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

    private List<DepartmentData> GetDepartments(List<Guid> departmentIds ,List<string> errors)
    {
      if (departmentIds == null || departmentIds.Count == 0)
      {
        return null;
      }

      const string errorMessage = "Can not get departments. Please try again later.";

      try
      {
        object request = IGetDepartmentsRequest.CreateObj(departmentIds);
        Response<IOperationResult<IGetDepartmentsResponse>> response = _departmentsRequestClient
          .GetResponse<IOperationResult<IGetDepartmentsResponse>>(request)
          .Result;

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
      IRequestClient<IGetDepartmentsRequest> departmentsRequestClient,
      IRequestClient<IGetUsersDataRequest> usersRequestClient,
      ILogger<GetNewsCommand> logger)
    {
      _repository = repository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
      _departmentsRequestClient = departmentsRequestClient;
      _usersRequestClient = usersRequestClient;
      _logger = logger;
    }

    public FindResultResponse<NewsInfo> Execute(FindNewsFilter findNewsFilter)
    {
      FindResultResponse<NewsInfo> response = new();

      List<DbNews> dbNewsList = _repository.Find(findNewsFilter, out int totalCount);

      List<Guid> departmentsIds = dbNewsList.Select(d => d.DepartmentId.Value).ToHashSet().ToList();
      List<Guid> authorsIds = dbNewsList.Select(a => a.AuthorId).ToHashSet().ToList();

      List<DepartmentData> departments = GetDepartments(departmentsIds, response.Errors);

      List<UserData> authors = GetAuthor(authorsIds, response.Errors);
      List<Guid> imagesIds = new();
      if (authors != null)
      {
        imagesIds.AddRange(authors.Where(u => u.ImageId.HasValue).Select(u => u.ImageId.Value).ToList());
      }

      response.Body = dbNewsList.Select(dbNews => _mapper.Map(dbNews, departments, authors)).ToList();

      response.TotalCount = totalCount;
      response.Status = response.Errors.Any()
        ? OperationResultStatusType.PartialSuccess
        : OperationResultStatusType.FullSuccess;

      if (response.Body == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

        response.Errors = new() { "News was not found." };
        response.Status = OperationResultStatusType.Failed;
      }

      return response;
    }
  }
}
