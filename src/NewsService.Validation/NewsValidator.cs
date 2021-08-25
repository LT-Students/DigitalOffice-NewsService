using FluentValidation;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using System;

namespace LT.DigitalOffice.NewsService.Validation
{
    public class NewsValidator : AbstractValidator<News>, INewsValidator
    {
        public NewsValidator()
        {
            RuleFor(news => news.AuthorId)
                .NotEmpty();

            RuleFor(news => news.Pseudonym)
                .MaximumLength(50)
                .WithMessage("Pseudonym is too long.");

            RuleFor(news => news.Subject)
                .NotEmpty()
                .MaximumLength(120)
                .WithMessage("News subject is too long.");

            RuleFor(news => news.Content)
                .NotEmpty();

            When(
                news => news.DepartmentId.HasValue,
                () =>
                    RuleFor(news => news.DepartmentId)
                        .Must(DepartmentId => DepartmentId != Guid.Empty)
                        .WithMessage("Wrong type of department Id."));
        }
    }
}
