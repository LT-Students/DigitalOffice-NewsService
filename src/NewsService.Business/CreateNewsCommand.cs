using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.NewsService.Business
{
  public class CreateNewsCommand : ICreateNewsCommand
  {
    private readonly INewsRepository _repository;
    private readonly IDbNewsMapper _mapper;
    private readonly INewsValidator _validator;
    private readonly IAccessValidator _accessValidator;
    private readonly IRequestClient<ICheckDepartmentsExistence> _rcCheckDepartmentsExistence;
    private readonly ILogger<CreateNewsCommand> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private List<Guid> CheckDepartmentExistence(Guid? departmentId, List<string> errors)
    {
      if (!departmentId.HasValue)
      {
        return null;
      }
      string errorMessage = "Failed to check the existing department.";
      string logMessage = "Department with id: {id} not found.";

      try
      {
        var response = _rcCheckDepartmentsExistence.GetResponse<IOperationResult<ICheckDepartmentsExistence>>(
          ICheckDepartmentsExistence.CreateObj(new List<Guid> { departmentId.Value })).Result;
        if (response.Message.IsSuccess)
        {
          if (!response.Message.Body.DepartmentIds.Any())
          {
            errors.Add($"Department Id: {departmentId} does not exist");
            return null;
          }
          return response.Message.Body.DepartmentIds;
        }

        _logger.LogWarning("Can not find department with this Id: {departmentId}: " +
          $"{Environment.NewLine}{string.Join('\n', response.Message.Errors)}");
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, logMessage);
      }

      errors.Add(errorMessage);
      return null;
    }

    public CreateNewsCommand(
      INewsRepository repository,
      IDbNewsMapper mapper,
      INewsValidator validator,
      IAccessValidator accessValidator,
      IRequestClient<ICheckDepartmentsExistence> rcCheckDepartmentsExistence,
      ILogger<CreateNewsCommand> logger,
      IHttpContextAccessor httpContextAccessor)
    {
      _repository = repository;
      _mapper = mapper;
      _validator = validator;
      _accessValidator = accessValidator;
      _rcCheckDepartmentsExistence = rcCheckDepartmentsExistence;
      _logger = logger;
      _httpContextAccessor = httpContextAccessor;
    }

    public OperationResultResponse<Guid?> Execute(CreateNewsRequest request)
    {
      if (!_accessValidator.HasRights(Rights.AddEditRemoveNews))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new OperationResultResponse<Guid?>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
        };
      }

      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<Guid?>
        {
          Status = OperationResultStatusType.Failed,
          Errors = errors
        };
      }

      OperationResultResponse<Guid?> response = new();

      List<Guid> existDepartments = CheckDepartmentExistence(request.DepartmentId, response.Errors);

      response.Body = _repository.Create(_mapper.Map(request, existDepartments));

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      response.Status = response.Errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess;

      if (response.Body == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
      }

      return response;
    }
  }
}
