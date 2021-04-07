using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
    public interface IFindNewsCommand
    {
        List<NewsResponse> Execute(FindNewsFilter findNewsFilter);
    }
}
