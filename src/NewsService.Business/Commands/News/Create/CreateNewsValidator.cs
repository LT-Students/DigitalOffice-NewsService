using System.Linq;
using FluentValidation;

namespace LT.DigitalOffice.NewsService.Business.Commands.News.Create
{
  public class CreateNewsValidator : AbstractValidator<CreateNewsRequest>
  {
    public CreateNewsValidator()
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
