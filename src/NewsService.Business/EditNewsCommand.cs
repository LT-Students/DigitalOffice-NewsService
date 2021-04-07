using FluentValidation;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.NewsService.Business
{
    public class EditNewsCommand : IEditNewsCommand
    {
        private readonly INewsRepository _repository;
        private readonly INewsMapper _mapper;
        private readonly IValidator<News> _validator;

        public EditNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] INewsMapper mapper,
            [FromServices] IValidator<News> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public void Execute(Guid newsId, JsonPatchDocument<EditNewsRequest> request)
        {

            //_validator.ValidateAndThrowCustom(request);


            //var news = _mapper.Map(request);

            //_repository.EditNews(news);
        }
    }
}
