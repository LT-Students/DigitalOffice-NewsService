using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;

namespace LT.DigitalOffice.NewsService.Validation.Channel.Interfaces
{
  [AutoInject]
  public interface ICreateChannelRequestValidator : IValidator<CreateChannelRequest>
  {
  }
}
