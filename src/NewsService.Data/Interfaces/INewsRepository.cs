using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Data.Interfaces
{
    [AutoInject]
    public interface INewsRepository
    {
        Guid? Create(DbNews news);

        bool Edit(Guid newsId, JsonPatchDocument<DbNews> news);

        DbNews Get(Guid newsId);

        List<DbNews> Find(FindNewsFilter findNewsFilter, int skipCount, int takeCount, List<string> errors, out int totalCount);
    }
}
