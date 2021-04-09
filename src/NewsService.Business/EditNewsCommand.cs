using FluentValidation;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Validation;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.NewsService.Business
{
    public class EditNewsCommand : IEditNewsCommand
    {

        private readonly INewsRepository _repository;
        private readonly IPatchNewsMapper _mapper;
        private readonly IPatchNewsValidator _validator;
        public EditNewsCommand(
            INewsRepository repository,
            IPatchNewsMapper mapper,
            IPatchNewsValidator validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public bool Execute(Guid newsId, JsonPatchDocument<EditNewsRequest> request)
        {
            _validator.ValidateAndThrowCustom(request);
            var dbEditNews = _mapper.Map(request);
            return _repository.EditNews(newsId, dbEditNews);
        }
    }
}
