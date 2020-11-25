using FluentValidation;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.NewsService.Business
{
    public class CreateNewsCommand : ICreateNewsCommand
    {
        private readonly INewsRepository repository;
        private readonly INewsMapper mapper;
        private readonly IValidator<News> validator;

        public CreateNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] INewsMapper mapper,
            [FromServices] IValidator<News> validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public Guid Execute(News request)
        {
            validator.ValidateAndThrowCustom(request);

            var news = mapper.Map(request);

            return repository.CreateNews(news);
        }
    }
}
