using System.Linq;
using FluentValidation;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;
using LT.DigitalOffice.NewsService.Validation.Interfaces;

namespace LT.DigitalOffice.NewsService.Validation
{
  public class CreateNewsRequestValidator : AbstractValidator<CreateNewsRequest>, ICreateNewsRequestValidator
  {
    public CreateNewsRequestValidator()
    {
      RuleFor(news => news.TagsIds)
        .Cascade(CascadeMode.Stop)
        .NotNull().WithMessage("Tags list must not be null.")
        .Must(t => t.Count() == t.Distinct().Count())
        .WithMessage("The tags can't be duplicated.");

      RuleFor(news => news.Subject)
        .NotEmpty().WithMessage("Subject must not be empty.");

      RuleFor(news => news.Preview)
        .NotEmpty().WithMessage("Preview must not be empty.");

      RuleFor(news => news.Content)
        .NotEmpty().WithMessage("Content must not be empty.");
    }
  }
}
