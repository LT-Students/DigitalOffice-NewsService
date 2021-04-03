using LT.DigitalOffice.NewsService.Models.Dto.ModelResponse;
using System;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for getting NewsResponse model by id.
    /// </summary>
    public interface IGetNewsByIdCommand
    {
        /// <summary>
        /// Returns the newsresponse model with the specified id.
        /// </summary>
        /// <param name="newsId">Specified id of news.</param>
        /// <returns>News model with specified id.</returns>
        NewsResponse Execute(Guid newsId);
    }
}
