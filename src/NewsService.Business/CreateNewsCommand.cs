using FluentValidation;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.NewsService.Business
{
    public class CreateNewsCommand : ICreateNewsCommand
    {
        private readonly INewsRepository repository;
        private readonly IMapper<News, DbNews> mapper;
        private readonly IValidator<News> validator;

        public CreateNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] IMapper<News, DbNews> mapper,
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
