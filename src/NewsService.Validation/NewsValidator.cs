﻿using FluentValidation;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Validation.Interfaces;

namespace LT.DigitalOffice.NewsService.Validation
{
    public class NewsValidator : AbstractValidator<News>, INewsValidator
    {
        public NewsValidator()
        {
            RuleFor(news => news.AuthorId)
                .NotEmpty();

            RuleFor(news => news.SenderId)
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
        }
    }
}
