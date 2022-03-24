using FluentValidation;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Tag;
using LT.DigitalOffice.NewsService.Validation.Tag.Interface;

namespace LT.DigitalOffice.NewsService.Validation.Tag
{
  public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>, ICreateTagRequestValidator
  {
    public CreateTagRequestValidator()
    {
      RuleFor(tag => tag.Name)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("Name must not be empty.")
        .MaximumLength(80).WithMessage("Tag is too long.");
    }
  }
}
