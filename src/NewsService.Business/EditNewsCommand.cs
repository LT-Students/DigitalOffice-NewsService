using FluentValidation;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Business
{
    public class EditNewsCommand : IEditNewsCommand
    {
        private readonly INewsRepository repository;
        private readonly IMapper<News, DbNews> mapper;
        private readonly IValidator<News> validator;

        public EditNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] IMapper<News, DbNews> mapper,
            [FromServices] IValidator<News> validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public void Execute(News request)
        {
            validator.ValidateAndThrowCustom(request);

            var news = mapper.Map(request);

            repository.EditNews(news);
        }
    }
}
