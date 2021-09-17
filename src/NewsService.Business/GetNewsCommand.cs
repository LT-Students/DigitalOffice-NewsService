using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.Company;
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
    private readonly IRequestClient<IGetUsersDataRequest> _usersRequestClient;
    private readonly IRequestClient<IGetDepartmentsRequest> _departmentsRequestClient;
    private readonly IDepartmentInfoMapper _departmentInfoMapper;
    private readonly ILogger<GetNewsCommand> _logger;

    private UserData GetAuthor(Guid userId, List<string> errors)
    {
      string errorMessage = "Cannot get author now. Please try again later.";
      const string logMessage = "Cannot get author with ids: {userId}.";

      try
      {
        IOperationResult<IGetUsersDataResponse> response = _usersRequestClient.GetResponse<IOperationResult<IGetUsersDataResponse>>(
          IGetUsersDataRequest.CreateObj(new List<Guid> { userId })).Result.Message;

        if (response.IsSuccess)
        {
          return response.Body.UsersData.FirstOrDefault();
        }

        _logger.LogWarning(logMessage, string.Join(", ", userId));
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, logMessage, string.Join(", ", userId));
      }

      errors.Add(errorMessage);

      return null;
    }

    private DepartmentInfo GetDepartment(Guid? departmentId, List<string> errors)
    {
      if (departmentId == null)
      {
        return null;
      }

      try
      {
        IOperationResult<IGetDepartmentsResponse> departmentResponse =
          _departmentsRequestClient.GetResponse<IOperationResult<IGetDepartmentsResponse>>(
            IGetDepartmentsRequest.CreateObj(new List<Guid> { departmentId.Value }))
          .Result.Message;

        if (departmentResponse.IsSuccess)
        {
          return _departmentInfoMapper.Map(departmentResponse.Body.Departments.FirstOrDefault());
        }

        _logger.LogWarning(
          $"Can not get department. Reason:{Environment.NewLine}{string.Join('\n', departmentResponse.Errors)}.");
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, "Exception on get department request.");
      }

      errors.Add($"Can not get department info for DepartmentId '{departmentId}'. Please try again later.");

      return null;
    }

    public GetNewsCommand(
      INewsRepository repository,
      INewsResponseMapper mapper,
      IHttpContextAccessor httpContextAccessor,
      IRequestClient<IGetDepartmentsRequest> departmentsRequestClient,
      IRequestClient<IGetUsersDataRequest> usersRequestClient,
      IDepartmentInfoMapper departmentMapper,
      ILogger<GetNewsCommand> logger)
    {
      _repository = repository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
      _departmentsRequestClient = departmentsRequestClient;
      _usersRequestClient = usersRequestClient;
      _departmentInfoMapper = departmentMapper;
      _logger = logger;
    }

    public OperationResultResponse<NewsResponse> Execute(Guid newsId)
    {
      OperationResultResponse<NewsResponse> response = new();

      DbNews dbNews = _repository.Get(newsId);
      if (dbNews == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
        return response;
      }

      DepartmentInfo department = null;
      if (dbNews.DepartmentId.HasValue)
      {
        department = GetDepartment(dbNews.DepartmentId, response.Errors);
      }

      UserData author = GetAuthor(dbNews.AuthorId, response.Errors);
      if (author == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
        return response;
      }

      response.Body = _mapper.Map(dbNews, department, author);

      response.Status = OperationResultStatusType.FullSuccess;

      if (response.Body == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

        response.Errors = new() { "Not news found." };
        response.Status = OperationResultStatusType.Failed;
      }

      return response;
    }
  }
}
