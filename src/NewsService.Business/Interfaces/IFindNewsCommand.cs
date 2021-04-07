using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
    public interface IFindNewsCommand
    {
        public List<NewsResponse> Execute(FindNewsParams findNewsParams);
    }
}
