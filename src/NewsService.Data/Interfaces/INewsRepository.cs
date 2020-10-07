using LT.DigitalOffice.NewsService.Models.Db;
using System;

namespace LT.DigitalOffice.NewsService.Data.Interfaces
{
    /// <summary>
    /// Represents interface of repository in repository pattern.
    /// Provides methods for working with the database of NewsService.
    /// </summary>
    public interface INewsRepository
    {
        /// <summary>
        /// Adds new news to the database.
        /// </summary>
        /// <param name="news">News to add.</param>
        /// <returns>Guid of added news.</returns>
        Guid CreateNews(DbNews news);
    }
}
