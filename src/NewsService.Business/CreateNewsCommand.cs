using FluentValidation;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Validation;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.NewsService.Business
{
    public class CreateNewsCommand : ICreateNewsCommand
    {
        private readonly INewsRepository _repository;
        private readonly INewsMapper _mapper;
        private readonly INewsValidator _validator;

        public CreateNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] INewsMapper mapper,
            [FromServices] INewsValidator validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public Guid Execute(News request)
        {
            _validator.ValidateAndThrowCustom(request);

            var news = _mapper.Map(request);

            return _repository.CreateNews(news);
        }
    }
}
