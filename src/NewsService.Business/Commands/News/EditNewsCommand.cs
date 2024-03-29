﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Business.Commands.News.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Business.Commands.News
{
  public class EditNewsCommand : IEditNewsCommand
  {
    private readonly INewsRepository _repository;
    private readonly IPatchNewsMapper _mapper;
    private readonly IEditNewsRequestValidator _validator;
    private readonly IAccessValidator _accessValidator;
    private readonly IResponseCreator _responseCreator;

    public EditNewsCommand(
      INewsRepository repository,
      IPatchNewsMapper mapper,
      IEditNewsRequestValidator validator,
      IAccessValidator accessValidator,
      IResponseCreator responseCreator)
    {
      _repository = repository;
      _mapper = mapper;
      _validator = validator;
      _accessValidator = accessValidator;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid newsId,
      JsonPatchDocument<EditNewsRequest> request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveNews))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResult = await _validator.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<bool>(
          HttpStatusCode.BadRequest, validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
      }

      OperationResultResponse<bool> response = new();

      response.Body = await _repository.EditAsync(newsId, _mapper.Map(request));

      if (!response.Body)
      {
        response = _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
