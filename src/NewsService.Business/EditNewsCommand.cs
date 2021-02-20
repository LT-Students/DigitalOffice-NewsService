using FluentValidation;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.NewsService.Business
{
    public class EditNewsCommand : IEditNewsCommand
    {
        private readonly INewsRepository _repository;
        private readonly IAddNewsChangesHistoryMapperRequest _mapper;
        private readonly IValidator<JsonPatchDocument<DbNews>> _validator;

        public EditNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] IAddNewsChangesHistoryMapperRequest mapper,
            [FromServices] IValidator<JsonPatchDocument<DbNews>> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public void Execute(Guid userId, Guid newsId, JsonPatchDocument<DbNews> patch)
        {

            _validator.ValidateAndThrowCustom(patch);

            var dbNews = _repository.GetNew(newsId);

            var dbNewsChangesHistory = _mapper.Map(patch);

            dbNewsChangesHistory.NewsId = newsId;
            dbNewsChangesHistory.ChangedBy = userId;

            if (dbNews == null)
            {
                throw new ArgumentNullException(nameof(dbNews));
            }

            patch.ApplyTo((DbNews)dbNews);

            _repository.EditNews(dbNews);
            _repository.CreateNewsHistory(dbNewsChangesHistory, newsId);
        }
    }
}
