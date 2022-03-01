using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;
using LT.DigitalOffice.NewsService.Validation.Channel.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;

namespace LT.DigitalOffice.NewsService.Validation.Channel
{
  public class EditChannelRequestValidator : BaseEditRequestValidator<EditChannelRequest>, IEditChannelRequestValidator
  {
    private List<string> AllowedExtensions = new()
    { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tga" };

    private void HandleInternalPropertyValidation(
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
          nameof(EditChannelRequest.CastomMessage),
          nameof(EditChannelRequest.Image),
          nameof(EditChannelRequest.IsActive)
        });

      AddСorrectOperations(nameof(EditChannelRequest.Name), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.CastomMessage), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.IsActive), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.Image), new() { OperationType.Replace });

      #endregion

      #region Name

      AddFailureForPropertyIf(
        nameof(EditChannelRequest.Name),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value.ToString()), "Name cannot be empty." },
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
                  AllowedExtensions.Contains(image.Extension))
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
        nameof(EditChannelRequest.CastomMessage),
        x => x == OperationType.Replace,
        new()
        {
          { x => x.value.ToString().Length < 120, "Subject is too long." },
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

    public EditChannelRequestValidator()
    {
      RuleForEach(x => x.Operations)
        .Custom(HandleInternalPropertyValidation);
    }
  }
}
