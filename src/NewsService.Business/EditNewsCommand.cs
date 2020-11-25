using FluentValidation;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Business
{
    public class EditNewsCommand : IEditNewsCommand
    {
        private readonly INewsRepository repository;
        private readonly INewsMapper mapper;
        private readonly IValidator<News> validator;

        public EditNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] INewsMapper mapper,
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
