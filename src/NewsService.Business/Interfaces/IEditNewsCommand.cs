using LT.DigitalOffice.NewsService.Models.Dto.Models;

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
        /// <param name="request">New news data.</param>
        void Execute(News request);
    }
}
