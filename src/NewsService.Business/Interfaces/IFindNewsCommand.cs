using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
    public interface IFindNewsCommand
    {
        public List<DbNews> Execute(FindNewsParams findNewsParams);
    }
}
