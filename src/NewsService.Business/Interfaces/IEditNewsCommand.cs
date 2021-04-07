using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for edit news.
    /// </summary>
    public interface IEditNewsCommand
    {
        /// <summary>
        /// Edits news.
        /// </summary>
        /// <param name="newsId">Unique identefer of news.</param>
        /// <param name="request">News value.</param>
        void Execute(Guid newsId, JsonPatchDocument<EditNewsRequest> request);
    }
}
