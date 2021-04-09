using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Data.Interfaces
{
    /// <summary>
    /// Represents interface of repository in repository pattern.
    /// Provides methods for working with the database of NewsService.
    /// </summary>
    [AutoInject]
    public interface INewsRepository
    {
        /// <summary>
        /// Adds new news to the database.
        /// </summary>
        /// <param name="news">News to add.</param>
        /// <returns>Guid of added news.</returns>
        Guid CreateNews(DbNews news);

        /// <summary>
        /// Update existing news in the database.
        /// </summary>
        /// <param name="news">News to edit.</param>
        void EditNews(DbNews news);

        /// <summary>
        /// Returns the DbNews with the specified id from database.
        /// </summary>
        /// <param name="newsId">Specified id of DbNews.</param>
        /// <returns>News with specified id.</returns>
        DbNews GetNewsInfoById(Guid newsId);

        /// <summary>
        /// Find news in the database.
        /// </summary>
        /// <param name="findNewsFilter">Filter for serch.</param>
        List<DbNews> FindNews(FindNewsFilter findNewsFilter);
    }
}
