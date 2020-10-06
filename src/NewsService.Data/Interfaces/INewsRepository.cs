using LT.DigitalOffice.NewsService.Models.Db;
using System;
using System.Collections.Generic;
using System.Text;

namespace LT.DigitalOffice.NewsService.Data.Interfaces
{
    public interface INewsRepository
    {
        DbNews GetNewsById(Guid newsId);
    }
}
