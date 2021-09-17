using System;
using FluentValidation;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.NewsService.Validation.Interfaces;

namespace LT.DigitalOffice.NewsService.Validation
{
  public class FindNewsFilterValidator : AbstractValidator<FindNewsFilter>, IFindNewsFilterValidator
  {
    public FindNewsFilterValidator()
    {
      RuleFor(news => news.SkipCount)
        .NotNull().WithMessage("Skip Count must not be null.")
        .Must(sc => sc < 0).WithMessage("Skip count can't be less than 0.");

      RuleFor(news => news.TakeCount)
        .NotNull().WithMessage("TakeCount must not be null.")
        .Must(tc => tc < 1).WithMessage("Take count can't be less than 1.");

      When(
        news => news.DepartmentId.HasValue,
        () =>
          RuleFor(news => news.DepartmentId)
            .Must(DepartmentId => DepartmentId != Guid.Empty)
            .WithMessage("Wrong type of department Id."));

      When(
        news => news.AuthorId.HasValue,
        () =>
          RuleFor(news => news.DepartmentId)
            .Must(authorId => authorId != Guid.Empty)
            .WithMessage("Wrong type of author Id."));
    }
  }
}
