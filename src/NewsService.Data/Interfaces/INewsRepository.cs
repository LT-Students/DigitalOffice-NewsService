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

        /// <summary>
        /// Update existing news in the database.
        /// </summary>
        /// <param name="news">News to edit.</param>
        void EditNews(DbNews news);

        /// <summary>
        /// Return existing New database model.
        /// </summary>
        /// <param name="newId">Specific New id.</param>
        DbNews GetNew(Guid newId);

        /// <summary>
        /// Add specific news history in database.
        /// </summary>
        /// <param name="dbnewsHistory">News .</param>
        /// <param name="newId">Specific New id.</param>
        void CreateNewsHistory(DbNewsChangesHistory dbnewsHistory, Guid newId);
    }
}
