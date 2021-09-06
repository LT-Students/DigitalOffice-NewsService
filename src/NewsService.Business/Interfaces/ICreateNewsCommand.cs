using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
  /// <summary>
  /// Represents interface for a command in command pattern.
  /// Provides method for adding new news.
  /// </summary>
  [AutoInject]
  public interface ICreateNewsCommand
  {
    /// <summary>
    ///  Adds new news.
    /// </summary>
    /// <param name="request">News data.</param>
    /// <returns>Guid of added news.</returns>
    OperationResultResponse<Guid> Execute(News request);
  }
}
