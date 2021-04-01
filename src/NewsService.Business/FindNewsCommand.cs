using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.ModelResponse;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Business
{
    public class FindNewsCommand: IFindNewsCommand
    {
        private readonly INewsRepository _repository;
        private readonly INewsResponseMapper _mapper;

        public FindNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] INewsResponseMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<NewsResponse> Execute(FindNewsParams findNewsParams)
        {
            List<DbNews> dbNewsList = _repository.FindNews(findNewsParams);
            List<NewsResponse> newsResponseList = new List<NewsResponse>();

            foreach(DbNews dbNews in dbNewsList)
            {
                newsResponseList.Add(_mapper.Map(dbNews));
            }

            return newsResponseList;
        }
    }
}
