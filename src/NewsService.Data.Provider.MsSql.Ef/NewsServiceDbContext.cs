using System.Reflection;
using LT.DigitalOffice.Kernel.Database;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef
{
  public class NewsServiceDbContext : DbContext, IDataProvider
  {
    public NewsServiceDbContext(DbContextOptions<NewsServiceDbContext> options)
      : base(options)
    {
    }

    public DbSet<DbNews> News { get; set; }

    // Fluent API is written here.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("LT.DigitalOffice.NewsService.Models.Db"));
    }

    public object MakeEntityDetached(object obj)
    {
      Entry(obj).State = EntityState.Detached;
      return Entry(obj).State;
    }

    void IBaseDataProvider.Save()
    {
      SaveChanges();
    }

    public void EnsureDeleted()
    {
      Database.EnsureDeleted();
    }

    public bool IsInMemory()
    {
      return Database.IsInMemory();
    }
  }
}
