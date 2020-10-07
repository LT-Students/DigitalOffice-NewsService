using LT.DigitalOffice.NewsService.Models.Dto;
using System;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding new news.
    /// </summary>
    public interface ICreateNewsCommand
    {
        /// <summary>
        ///  Adds new news.
        /// </summary>
        /// <param name="request">News data.</param>
        /// <returns>Guid of added news.</returns>
        Guid Execute(CreateNewsRequest request);
    }
}
