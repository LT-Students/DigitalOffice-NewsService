using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LT.DigitalOffice.NewsService.Data.Provider
{
    public interface IDataProvider
    {
        DbSet<DbNewsFile> NewsFiles { get; set; }
        DbSet<DbNews> News { get; set; }

        void Save();
        object MakeEntityDetached(object obj);
        void EnsureDeleted();
        bool IsInMemory();
    }
}
