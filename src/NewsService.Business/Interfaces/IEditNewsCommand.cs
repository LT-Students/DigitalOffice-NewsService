using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
  /// <summary>
  /// Represents interface for a command in command pattern.
  /// Provides method for edit news.
  /// </summary>
  [AutoInject]
  public interface IEditNewsCommand
  {
    /// <summary>
    /// Edits news.
    /// </summary>
    /// <param name="newsId">Unique identefer of news.</param>
    /// <param name="request">News value.</param>
    OperationResultResponse<bool> Execute(Guid newsId, JsonPatchDocument<EditNewsRequest> request);
  }
}
