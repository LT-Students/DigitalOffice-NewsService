using FluentValidation;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Business
{
    public class EditNewsCommand : IEditNewsCommand
    {
        private readonly INewsRepository _repository;
        private readonly INewsMapper _mapper;
        private readonly INewsValidator _validator;

        public EditNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] INewsMapper mapper,
            [FromServices] INewsValidator validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public void Execute(News request)
        {
            _validator.ValidateAndThrowCustom(request);

            var news = _mapper.Map(request);

            _repository.EditNews(news);
        }
    }
}
