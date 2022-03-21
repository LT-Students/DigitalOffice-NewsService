using FluentValidation;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Tag;
using LT.DigitalOffice.NewsService.Validation.Tag.Interface;

namespace LT.DigitalOffice.NewsService.Validation.Tag
{
  public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>, ICreateTagRequestValidator
  {
    public CreateTagRequestValidator()
    {
      When(
        tag => !string.IsNullOrEmpty(tag.Name),
        () =>
        RuleFor(tag => tag.Name)
          .MaximumLength(80).WithMessage("Tag is too long."));
    }
  }
}
