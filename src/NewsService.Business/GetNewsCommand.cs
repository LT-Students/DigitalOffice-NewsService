using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Models;
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
    private readonly ILogger<GetNewsCommand> _logger;

    private UserData GetAuthor(Guid userId, List<string> errors)
    {
      try
      {
        IOperationResult<IGetUsersDataResponse> response = _rcGetUsers.GetResponse<IOperationResult<IGetUsersDataResponse>>(
          IGetUsersDataRequest.CreateObj(new List<Guid> { userId })).Result.Message;

        if (response.IsSuccess)
        {
          return response.Body.UsersData.FirstOrDefault();
        }

        _logger.LogWarning(
          "Can not get author. Reason:{errors}", string.Join('\n', response.Errors));
      }
      catch (Exception exc)
      {
        _logger.LogError("Exception on get author request. {errorsMessage}", exc.Message);
      }

      errors.Add($"Can not get author info for authorId '{userId}'. Please try again later.");

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
          _rcGetDepartments.GetResponse<IOperationResult<IGetDepartmentsResponse>>(
            IGetDepartmentsRequest.CreateObj(new List<Guid> { departmentId.Value }))
          .Result.Message;

        if (departmentResponse.IsSuccess)
        {
          return _departmentInfoMapper.Map(departmentResponse.Body.Departments.FirstOrDefault());
        }

        _logger.LogWarning(
          "Can not get department. Reason:{errors}", string.Join('\n', departmentResponse.Errors));
      }
      catch (Exception exc)
      {
        _logger.LogError("Exception on get department request. {errorsMessage}", exc.Message);
      }

      errors.Add($"Can not get department info for DepartmentId '{departmentId}'. Please try again later.");

      return null;
    }

    private ImageData GetAuthorAvatarImage(Guid? imageId, List<string> errors)
    {
      try
      {
        IOperationResult<IGetImagesResponse> response = _rcGetUsers.GetResponse<IOperationResult<IGetImagesResponse>>(
          IGetImagesRequest.CreateObj(new List<Guid> { imageId.Value }, ImageSource.News)).Result.Message;

        if (response.IsSuccess)
        {
          return response.Body.ImagesData.FirstOrDefault();
        }

        _logger.LogWarning(
          "Can not get image. Reason:{errors}", string.Join('\n', response.Errors));
      }
      catch (Exception exc)
      {
        _logger.LogError("Exception on get image request. {errorsMessage}", exc.Message);
      }

      errors.Add($"Can not get image info for imageId '{imageId}'. Please try again later.");

      return null;
    }

    public GetNewsCommand(
      INewsRepository repository,
      INewsResponseMapper mapper,
      IHttpContextAccessor httpContextAccessor,
      IRequestClient<IGetDepartmentsRequest> rcGetDepartments,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      IDepartmentInfoMapper departmentMapper,
      ILogger<GetNewsCommand> logger)
    {
      _repository = repository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
      _rcGetDepartments = rcGetDepartments;
      _rcGetUsers = rcGetUsers;
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

        response.Errors = new() { "News was not found." };
        response.Status = OperationResultStatusType.Failed;
        return response;
      }

      DepartmentInfo department = dbNews.DepartmentId.HasValue ? GetDepartment(dbNews.DepartmentId, response.Errors) : null;

      UserData author = GetAuthor(dbNews.AuthorId, response.Errors);
      Guid? imageId = author?.ImageId;
      ImageData avatarImage = GetAuthorAvatarImage(imageId, response.Errors);

      response.Body = _mapper.Map(dbNews, department, author, avatarImage);
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
