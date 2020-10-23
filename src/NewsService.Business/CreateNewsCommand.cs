using FluentValidation;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.NewsService.Business
{
    public class CreateNewsCommand : ICreateNewsCommand
    {
        private readonly INewsRepository repository;
        private readonly IMapper<NewsRequest, DbNews> mapper;
        private readonly IValidator<NewsRequest> validator;

        public CreateNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] IMapper<NewsRequest, DbNews> mapper,
            [FromServices] IValidator<NewsRequest> validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public Guid Execute(NewsRequest request)
        {
            validator.ValidateAndThrowCustom(request);

            var news = mapper.Map(request);

            return repository.CreateNews(news);
        }
    }
}
