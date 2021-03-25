using LT.DigitalOffice.NewsService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for getting user model by id.
    /// </summary>
    public interface IGetNewsByIdCommand
    {
        /// <summary>
        /// Returns the user model with the specified id.
        /// </summary>
        /// <param name="newsId">Specified id of user.</param>
        /// <returns>User model with specified id.</returns>
        News Execute(Guid newsId);
    }
}
