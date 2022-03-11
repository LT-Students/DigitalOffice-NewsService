using FluentValidation;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;
using LT.DigitalOffice.NewsService.Validation.Channel.Interfaces;

namespace LT.DigitalOffice.NewsService.Validation.Channel
{
  public class CreateChannelRequestValidator : AbstractValidator<CreateChannelRequest>, ICreateChannelRequestValidator
  {
    public CreateChannelRequestValidator(
      IImageContentValidator imageContentValidator,
      IImageExtensionValidator imageExtensionValidator,
      IChannelRepository channelRepository,
      INewsRepository newsRepository)
    {
      RuleFor(c => c.Name)
        .NotEmpty().WithMessage("Name must not be empty.")
        .MustAsync(async (request, _) => !await channelRepository.DoesNameExistAsync(request))
        .WithMessage("The channel name is already exists.");

      When(c => c.PinnedNewsId != null, () =>
      {
        RuleFor(c => c.PinnedNewsId)
          .NotEmpty().WithMessage("News must not be empty.");
      });

      When(c => c.Image != null, () =>
      {
        RuleFor(c => c.Image.Content)
          .SetValidator(imageContentValidator);

        RuleFor(c => c.Image.Extension)
          .SetValidator(imageExtensionValidator);
      });
    }
  }
}
