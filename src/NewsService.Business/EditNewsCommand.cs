using FluentValidation;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.NewsService.Business
{
    public class EditNewsCommand : IEditNewsCommand
    {
        private readonly INewsRepository _repository;
        private readonly IAddNewsChangesHistoryMapperRequest _mapper;
        private readonly IValidator<News> _validator;

        public EditNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] IAddNewsChangesHistoryMapperRequest mapper,
            [FromServices] IValidator<News> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public void Execute(Guid userId, Guid newsId, JsonPatchDocument<DbNews> patch)
        {

            //validator.ValidateAndThrowCustom(request);

            var dbNews = _repository.GetNew(newsId);

            var dbNewsChangesHistory = _mapper.Map(patch);

            dbNewsChangesHistory.NewsId = newsId;
            dbNewsChangesHistory.ChangedBy = userId;

            patch.ApplyTo((DbNews)dbNews);

            _repository.EditNews(dbNews);
            _repository.CreateNewsHistory(dbNewsChangesHistory, newsId);
        }
    }
}
