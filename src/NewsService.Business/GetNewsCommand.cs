using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace LT.DigitalOffice.NewsService.Business
{
    public class GetNewsCommand : IGetNewsCommand
    {
        private readonly INewsRepository repository;
        private readonly IMapper<DbNews, News> mapper;

        public GetNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] IMapper<DbNews, News> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public News Execute(Guid newsId)
        {
            var dbNews = repository.GetNews(newsId);

            return mapper.Map(dbNews);
        }
    }
}
