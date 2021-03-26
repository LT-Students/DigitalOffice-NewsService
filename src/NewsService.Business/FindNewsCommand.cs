using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.NewsService.Business
{
    public class FindNewsCommand: IFindNewsCommand
    {
        private readonly INewsRepository _repository;
        private readonly INewsMapper _mapper;

        public FindNewsCommand(
            [FromServices] INewsRepository repository,
            [FromServices] INewsMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<DbNews> Execute(FindNewsParams findNewsParams)
        {
            return new List<DbNews>(); //_repository.FindNews(findNewsParams);
        }
    }
}
