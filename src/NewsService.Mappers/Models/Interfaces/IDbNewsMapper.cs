using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IDbNewsMapper
    {
        DbNews Map(News request, List<Guid> departmentId);
    }
}
