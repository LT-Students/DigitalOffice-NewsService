using LT.DigitalOffice.Kernel.Database;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.NewsService.Data.Provider
{
    public interface IDataProvider : IBaseDataProvider
    {
        public DbSet<DbNews> News { get; set; }
    }
}
