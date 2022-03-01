using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Business.Commands.Channels.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;
using LT.DigitalOffice.NewsService.Validation.Channel.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Business.Commands.Channels
{
  public class EditChannelCommand : IEditChannelCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEditChannelRequestValidator _validator;
    private readonly IChannelRepository _repository;
    private readonly IPatchChannelMapper _mapper;
    public EditChannelCommand(
      IAccessValidator accessValidator,
      IHttpContextAccessor httpContextAccessor,
      IEditChannelRequestValidator validator,
      IChannelRepository repository,
      IPatchChannelMapper mapper)
    {
      _accessValidator = accessValidator;
      _httpContextAccessor = httpContextAccessor;
      _validator = validator;
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid chanelId,
      JsonPatchDocument<EditChannelRequest> request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveNews))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
        };
      }

      ValidationResult validationResult = await _validator.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = validationResult.Errors.Select(vf => vf.ErrorMessage).ToList()
        };
      }
      OperationResultResponse<bool> response = new();

      response.Body = await _repository.EditAsync(chanelId, await _mapper.MapAsync(request));
      response.Status = OperationResultStatusType.FullSuccess;

      if (!response.Body)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
      }

      return response;
    }
  }
}
