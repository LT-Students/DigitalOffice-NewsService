using System;
using FluentValidation;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Validation.Interfaces;

namespace LT.DigitalOffice.NewsService.Validation
{
  public class CreateNewsRequestValidator : AbstractValidator<CreateNewsRequest>, ICreateNewsRequestValidator
  {
    public CreateNewsRequestValidator()
    {
      RuleFor(news => news.AuthorId)
        .NotEmpty().WithMessage("AuthorId must not be empty.");

      RuleFor(news => news.Pseudonym)
        .MaximumLength(50).WithMessage("Pseudonym is too long.");

      RuleFor(news => news.Subject)
        .NotEmpty().WithMessage("Subject must not be empty.")
        .MaximumLength(120).WithMessage("News subject is too long.");

      RuleFor(preview => preview.Preview)
        .NotEmpty().WithMessage("Preview must not be empty.");

      RuleFor(news => news.Content)
        .NotEmpty().WithMessage("Content must not be empty.");

      RuleFor(news => news.AuthorId)
        .NotEmpty().WithMessage("AuthorId must not be empty.");

      When(
        news => news.DepartmentId.HasValue,
        () =>
          RuleFor(news => news.DepartmentId)
            .Must(DepartmentId => DepartmentId != Guid.Empty)
            .WithMessage("Wrong type of department Id."));
    }
  }
}
