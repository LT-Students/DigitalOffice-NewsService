using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.NewsService.Business
{
    public class EditNewsCommand : IEditNewsCommand
    {

        private readonly INewsRepository _repository;
        private readonly IPatchNewsMapper _mapper;
        private readonly IEditNewsValidator _validator;
        public EditNewsCommand(
            INewsRepository repository,
            IPatchNewsMapper mapper,
            IEditNewsValidator validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public bool Execute(Guid newsId, JsonPatchDocument<EditNewsRequest> request)
        {
            _validator.ValidateAndThrowCustom(request);

            var dbRequest = _mapper.Map(request);

            return _repository.EditNews(newsId, dbRequest);
        }
    }
}
