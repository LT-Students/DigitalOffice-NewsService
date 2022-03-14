using System;
using System.Collections.Generic;
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
using LT.DigitalOffice.NewsService.Mappers.Db.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.NewsService.Business.Commands.News
{
  public class CreateNewsCommand : ICreateNewsCommand
  {
    private readonly INewsRepository _repository;
    private readonly IDbNewsMapper _mapper;
    private readonly ICreateNewsRequestValidator _validator;
    private readonly IAccessValidator _accessValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;
    private readonly IDbNewsTagMapper _newsTagsMapper;
    private readonly INewsTagRepository _newsTagsRepository;

    public CreateNewsCommand(
      INewsRepository repository,
      IDbNewsMapper mapper,
      ICreateNewsRequestValidator validator,
      IAccessValidator accessValidator,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator,
      IDbNewsTagMapper newsTagsMapper,
      INewsTagRepository newsTagsRepository)
    {
      _repository = repository;
      _mapper = mapper;
      _validator = validator;
      _accessValidator = accessValidator;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
      _newsTagsMapper = newsTagsMapper;
      _newsTagsRepository = newsTagsRepository;
    }

    public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateNewsRequest request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveNews))
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResult = await _validator.ValidateAsync(request);
      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<Guid?>(
          HttpStatusCode.BadRequest,
          validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
      }

      OperationResultResponse<Guid?> response = new();

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      response.Body = await _repository.CreateAsync(_mapper.Map(request));

      if (response.Body is null)
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest);
      }

      if (request.TagsIds.Any())
      {
        _newsTagsRepository.CreateAsync(
          _newsTagsMapper.Map(request.TagsIds.Distinct().ToList(),
          response.Body.Value));
      }

      return response;
    }
  }
}
