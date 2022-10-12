using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;
using LT.DigitalOffice.NewsService.Validation.Channel.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;

namespace LT.DigitalOffice.NewsService.Validation.Channel
{
  public class EditChannelRequestValidator : BaseEditRequestValidator<EditChannelRequest>, IEditChannelRequestValidator
  {
    private readonly IChannelRepository _channelRepository;
    private readonly INewsRepository _newsRepository;
    private readonly IImageContentValidator _imageContentValidator;
    private readonly IImageExtensionValidator _imageExtensionValidator;

    private async Task HandleInternalPropertyValidation(
      Operation<EditChannelRequest> requestedOperation,
      ValidationContext<JsonPatchDocument<EditChannelRequest>> context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region Paths

      AddСorrectPaths(
        new()
        {
          nameof(EditChannelRequest.Name),
          nameof(EditChannelRequest.PinnedMessage),
          nameof(EditChannelRequest.PinnedNewsId),
          nameof(EditChannelRequest.Image),
          nameof(EditChannelRequest.IsActive)
        });

      AddСorrectOperations(nameof(EditChannelRequest.Name), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.PinnedMessage), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.PinnedNewsId), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.IsActive), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.Image), new() { OperationType.Replace });

      #endregion

      #region Name

      AddFailureForPropertyIf(
        nameof(EditChannelRequest.Name),
        x => x == OperationType.Replace,
        new()
        {
          { x => string.IsNullOrEmpty(x.value.ToString()), "Name value must not be empty." }
        });

      await AddFailureForPropertyIfAsync(
        nameof(EditChannelRequest.Name),
        x => x == OperationType.Replace,
        new()
        {
          {
            async x =>
            !await _channelRepository.DoesNameExistAsync(x.value.ToString()),
            "This channel name already exist."
          }
        });

      #endregion

      #region Image

      AddFailureForPropertyIf(
        nameof(EditChannelRequest.Image),
        x => x == OperationType.Replace,
        new Dictionary<Func<Operation<EditChannelRequest>, bool>, string>
      {
        { x =>
          {
            ImageConsist image = JsonConvert.DeserializeObject<ImageConsist>(x.value?.ToString());

            return image is null
              ? true
              : _imageContentValidator.Validate(image.Content).IsValid &&
                _imageExtensionValidator.Validate(image.Extension).IsValid;
          },
            "Incorrect Image format"
          }
        });

      #endregion

      #region CastomMessage

      AddFailureForPropertyIf(
        nameof(EditChannelRequest.PinnedMessage),
        x => x == OperationType.Replace,
        new()
        {
          { x => x.value.ToString().Length < 120, "PinnedMessage is too long." },
        });

      #endregion

      #region PinnedNewsId

      AddFailureForPropertyIf(
        nameof(EditChannelRequest.PinnedNewsId),
        x => x == OperationType.Replace,
        new()
        {
          { x => Guid.TryParse(x.value.ToString(), out Guid _), "Incorrect format of NewsId." },
        });

      await AddFailureForPropertyIfAsync(
        nameof(EditChannelRequest.PinnedNewsId),
        x => x == OperationType.Replace,
        new()
        {
          {
            async x =>
            Guid.TryParse(x.value.ToString(), out Guid id)
            ? await _newsRepository.DoesNewsExistAsync(Guid.Parse(x.value.ToString()))
            : true,
            "This news doesn't exist."
          }
        });

      #endregion

      #region IsActive

      AddFailureForPropertyIf(
        nameof(EditChannelRequest.IsActive),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x.value?.ToString(), out _), "Incorrect IsActive format." },
        });

      #endregion
    }

    public EditChannelRequestValidator(
      IChannelRepository channelRepository,
      INewsRepository newsRepository,
      IImageContentValidator imageContentValidator,
      IImageExtensionValidator imageExtensionValidator)
    {
      _channelRepository = channelRepository;
      _newsRepository = newsRepository;
      _imageContentValidator = imageContentValidator;
      _imageExtensionValidator = imageExtensionValidator;

      RuleForEach(x => x.Operations)
        .CustomAsync(async (x, context, token) => await HandleInternalPropertyValidation(x, context));
    }
  }
}
