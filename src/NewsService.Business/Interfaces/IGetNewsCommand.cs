using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for getting news.
    /// </summary>
    public interface IGetNewsCommand
    {
        /// <summary>
        /// Get news by Id.
        /// </summary>
        /// <param name="newsId">Id of news.</param>
        /// <returns>News with specified Id.</returns>
        News Execute(Guid newsId);
    }
}
