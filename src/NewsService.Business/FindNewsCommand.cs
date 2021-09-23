﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
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
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IRequestClient<IGetDepartmentsRequest> _rcGetDepartments;
    private readonly ILogger<GetNewsCommand> _logger;
    private readonly IBaseFindRequestValidator _baseFindValidator;

    private async Task<List<UserData>> GetAuthors(List<Guid> authorIds, List<string> errors)
    {
      if (authorIds == null || authorIds.Count == 0)
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
      ILogger<GetNewsCommand> logger,
      IBaseFindRequestValidator baseFindValidator)
    {
      _repository = repository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
      _rcGetDepartments = rcGetDepartments;
      _rcGetUsers = rcGetUsers;
      _logger = logger;
      _baseFindValidator = baseFindValidator;
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

      List<Guid> departmentsIds = dbNewsList.Where(d => d.DepartmentId.HasValue).Select(d => d.DepartmentId.Value).Distinct().ToList();
      List<DepartmentData> departments = await GetDepartments(departmentsIds, response.Errors);

      List<Guid> authorsIds = dbNewsList.Select(a => a.AuthorId).Distinct().ToList();
      List<UserData> authors = await GetAuthors(authorsIds, response.Errors);

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
