using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Validation.Channel.Interfaces
{
  [AutoInject]
  public interface IEditChannelRequestValidator : IValidator<JsonPatchDocument<EditChannelRequest>>
  {
  }
}
