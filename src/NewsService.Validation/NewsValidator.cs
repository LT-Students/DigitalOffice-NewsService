using FluentValidation;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Validation
{
    public class NewsValidator : AbstractValidator<News>
    {
        public NewsValidator()
        {
            RuleFor(news => news.AuthorId)
                .NotEmpty();

            RuleFor(news => news.AuthorName)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Author name is too long.");

            RuleFor(news => news.SenderId)
                .NotEmpty();

            RuleFor(news => news.Subject)
                .NotEmpty()
                .MaximumLength(120)
                .WithMessage("News subject is too long.");

            RuleFor(news => news.Content)
                .NotEmpty();
        }
    }
}
