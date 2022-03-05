using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;
using LT.DigitalOffice.NewsService.Validation.Channel.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;

namespace LT.DigitalOffice.NewsService.Validation.Channel
{
  public class EditChannelRequestValidator : BaseEditRequestValidator<EditChannelRequest>, IEditChannelRequestValidator
  {
    private readonly IChannelRepository _channelRepository;
    private readonly INewsRepository _newsRepository;
    private async Task HandleInternalPropertyValidation(
      Operation<EditChannelRequest> requestedOperation,
      CustomContext context)
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
          { async x =>
            !await _channelRepository.DoesNameExistAsync(x.value.ToString()),
            "This channel name already exist."
          }
        });

      #endregion

      #region Image

      AddFailureForPropertyIf(
        nameof(EditChannelRequest.Image),
        x => x == OperationType.Replace,
        new()
        {
          {
            x =>
            {
              try
              {
                ImageConsist image = JsonConvert.DeserializeObject<ImageConsist>(x.value?.ToString());

                var byteString = new Span<byte>(new byte[image.Content.Length]);

                if (!String.IsNullOrEmpty(image.Content) &&
                  Convert.TryFromBase64String(image.Content, byteString, out _) &&
                  ImageFormats.formats.Contains(image.Extension))
                {
                  return true;
                }
              }
              catch
              {
              }
              return false;
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
            !await _newsRepository.DoesNewsExistAsync(Guid.Parse(x.value.ToString())),
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
      INewsRepository newsRepository)
    {
      _channelRepository = channelRepository;
      _newsRepository = newsRepository;

      RuleForEach(x => x.Operations)
        .CustomAsync(async (x, context, token) => await HandleInternalPropertyValidation(x, context));
    }
  }
}
