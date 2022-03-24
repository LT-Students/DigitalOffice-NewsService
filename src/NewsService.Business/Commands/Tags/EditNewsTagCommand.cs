using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Business.Commands.Tags.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Db.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Tag;
using LT.DigitalOffice.NewsService.Validation.Tag.Interface;

namespace LT.DigitalOffice.NewsService.Business.Commands.Tags
{
  public class EditNewsTagCommand : IEditNewsTagCommand
  {
    private readonly INewsTagRepository _newsTagsRepository;
    private readonly IDbNewsTagMapper _newsTagsMapper;
    private readonly ITagRepository _tagRepository;
    private readonly IResponseCreator _responseCreator;
    private readonly IEditTagRequestValidator _validator;
    private readonly IAccessValidator _accessValidator;

    public EditNewsTagCommand(
      INewsTagRepository newsTagsRepository,
      IDbNewsTagMapper newsTagsMapper,
      ITagRepository tagRepository,
      IResponseCreator responseCreator,
      IEditTagRequestValidator validator,
      IAccessValidator accessValidator)
    {
      _newsTagsRepository = newsTagsRepository;
      _newsTagsMapper = newsTagsMapper;
      _tagRepository = tagRepository;
      _responseCreator = responseCreator;
      _validator = validator;
      _accessValidator = accessValidator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid newsId, EditNewsTagsRequest request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveNews))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResult = await _validator.ValidateAsync((newsId, request));

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<bool>(
          HttpStatusCode.BadRequest,
          validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
      }

      OperationResultResponse<bool> response = new();

      if (request.TagsToRemove.Any())
      {
        await _newsTagsRepository.RemoveAsync(newsId, request.TagsToRemove);

        await _tagRepository.DowngradeCountAsync(request.TagsToRemove);
      }

      if (request.TagsToAdd.Any())
      {
        await _newsTagsRepository.CreateAsync(_newsTagsMapper.Map(request.TagsToAdd, newsId));

        await _tagRepository.UpdateCountAsync(request.TagsToAdd);
      }

      response.Body = true;

      return response;
    }
  }
}
