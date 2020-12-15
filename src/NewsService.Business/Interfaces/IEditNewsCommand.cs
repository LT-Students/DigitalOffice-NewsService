using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding new news.
    /// </summary>
    public interface IEditNewsCommand
    {
        /// <summary>
        /// Edits news.
        /// </summary>
        /// <param name="userId">Id of the requesting user.</param>
        /// <param name="newId">Existing news Id.</param>
        /// <param name="patch">News data subject to change.</param>
        void Execute(Guid userId, Guid newId, JsonPatchDocument<DbNews> patch);
    }
}
