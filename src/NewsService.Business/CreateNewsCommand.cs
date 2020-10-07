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
        private readonly IMapper<CreateNewsRequest, DbNews> mapper;

        public CreateNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] IMapper<CreateNewsRequest, DbNews> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Guid Execute(CreateNewsRequest request)
        {
            var news = mapper.Map(request);

            return repository.CreateNews(news);
        }
    }
}
