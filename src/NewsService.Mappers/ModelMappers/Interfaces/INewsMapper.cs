﻿using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.ModelMappers.Interfaces
{
    public interface INewsMapper : IMapper<News, DbNews>
    {
    }
}
