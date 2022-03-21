using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Business.Commands.Channels.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;
using LT.DigitalOffice.NewsService.Validation.Channel.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Business.Commands.Channels
{
  public class EditChannelCommand : IEditChannelCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IEditChannelRequestValidator _validator;
    private readonly IChannelRepository _repository;
    private readonly IPatchChannelMapper _mapper;
    private readonly IResponseCreator _responseCreator;

    public EditChannelCommand(
      IAccessValidator accessValidator,
      IEditChannelRequestValidator validator,
      IChannelRepository repository,
      IPatchChannelMapper mapper,
      IResponseCreator responseCreator)
    {
      _accessValidator = accessValidator;
      _validator = validator;
      _repository = repository;
      _mapper = mapper;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid channelId,
      JsonPatchDocument<EditChannelRequest> request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveNews))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResult = await _validator.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest,
          validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
      }

      OperationResultResponse<bool> response = new();

      response.Body = await _repository.EditAsync(channelId, await _mapper.MapAsync(request));

      if (!response.Body)
      {
        response = _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
