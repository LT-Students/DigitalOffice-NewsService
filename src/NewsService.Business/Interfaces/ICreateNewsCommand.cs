﻿using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using System;

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
        Guid Execute(News request);
    }
}
