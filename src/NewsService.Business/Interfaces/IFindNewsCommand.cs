using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
    [AutoInject]
    public interface IFindNewsCommand
    {
        /// <summary>
        /// Fined news.
        /// </summary>
        /// <param name="findNewsFilter">Model witch filtres.</param>
        List<NewsResponse> Execute(FindNewsFilter findNewsFilter);
    }
}
