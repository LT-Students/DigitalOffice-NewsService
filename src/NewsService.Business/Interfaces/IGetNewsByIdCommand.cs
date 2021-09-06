﻿using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
  /// <summary>
  /// Represents interface for a command in command pattern.
  /// Provides method for getting NewsResponse model by id.
  /// </summary>
  [AutoInject]
  public interface IGetNewsByIdCommand
  {
    /// <summary>
    /// Returns the NewsResponse model with the specified id.
    /// </summary>
    /// <param name="newsId">Specified id of news.</param>
    /// <returns>News model with specified id.</returns>
    OperationResultResponse<NewsResponse> Execute(Guid newsId);
  }
}
