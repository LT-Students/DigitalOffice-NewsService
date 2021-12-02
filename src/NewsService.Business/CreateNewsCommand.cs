using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Requests.Department;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
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
    private readonly ICreateNewsRequestValidator _validator;
    private readonly IAccessValidator _accessValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRequestClient<ICreateDepartmentEntityRequest> _rcCreateDepartmentEntity;
    private readonly ILogger<CreateNewsCommand> _logger;

    private async Task CreateDepartmentNews(Guid departmentId, Guid newsId, List<string> errors)
    {
      try
      {
        Response<IOperationResult<bool>> response =
          await _rcCreateDepartmentEntity.GetResponse<IOperationResult<bool>>(
            ICreateDepartmentEntityRequest.CreateObj(
              departmentId,
              _httpContextAccessor.HttpContext.GetUserId(),
              newsId: newsId));

        if (response.Message.IsSuccess && response.Message.Body)
        {
          return;
        }

        _logger.LogWarning(
          "Error while adding news id '{NewsId}' to department id '{DepartmentId}'.\n Errors: {Errors}",
          departmentId,
          newsId,
          string.Join('\n', response.Message.Errors));
      }
      catch (Exception exc)
      {
        _logger.LogError(
          exc,
          "Can not add news id '{NewsId}' to department id '{DepartmentId}'",
          departmentId,
          newsId);
      }

      errors.Add("Can not add department to the news.Please try again later.");
    }

    public CreateNewsCommand(
      INewsRepository repository,
      IDbNewsMapper mapper,
      ICreateNewsRequestValidator validator,
      IAccessValidator accessValidator,
      IHttpContextAccessor httpContextAccessor,
      IRequestClient<ICreateDepartmentEntityRequest> rcCreateDepartmentEntity,
      ILogger<CreateNewsCommand> logger)
    {
      _repository = repository;
      _mapper = mapper;
      _validator = validator;
      _accessValidator = accessValidator;
      _httpContextAccessor = httpContextAccessor;
      _rcCreateDepartmentEntity = rcCreateDepartmentEntity;
      _logger = logger;
    }

    public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateNewsRequest request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveNews))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new OperationResultResponse<Guid?>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
        };
      }

      ValidationResult validationResult = await _validator.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<Guid?>
        {
          Status = OperationResultStatusType.Failed,
          Errors = validationResult.Errors.Select(vf => vf.ErrorMessage).ToList()
        };
      }

      OperationResultResponse<Guid?> response = new();

      response.Body = await _repository.CreateAsync(_mapper.Map(request));

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      response.Status = OperationResultStatusType.FullSuccess;

      if (response.Body == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;

        return response;
      }

      if (request.DepartmentId.HasValue)
      {
        await CreateDepartmentNews(request.DepartmentId.Value, response.Body.Value, response.Errors);
      }

      response.Status = response.Errors.Any()
        ? OperationResultStatusType.PartialSuccess
        : OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
