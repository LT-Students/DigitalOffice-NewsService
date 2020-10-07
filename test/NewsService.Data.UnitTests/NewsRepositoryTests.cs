using LT.DigitalOffice.Kernel.UnitTestLibrary;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.NewsService.Data.UnitTests
{
    class NewsRepositoryTests
    {
        private IDataProvider provider;
        private INewsRepository repository;

        private DbNews dbNewsRequest;
        private DbNews dbNews;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<NewsServiceDbContext>()
                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                   .Options;
            provider = new NewsServiceDbContext(dbOptions);

            repository = new NewsRepository(provider);
        }

        [SetUp]
        public void SetUp()
        {
            dbNews = new DbNews
            {
                Id = Guid.NewGuid(),
                Content = "Content",
                Subject = "Subject",
                AuthorName = "AuthorName",
                AuthorId = Guid.NewGuid(),
                SenderId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            provider.News.Add(dbNews);
            provider.Save();

            dbNewsRequest = new DbNews
            {
                Id = dbNews.Id,
                Content = "Content111",
                Subject = "Subject111",
                AuthorName = "AuthorName111",
                AuthorId = Guid.NewGuid(),
                SenderId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

        }

        [TearDown]
        public void CleanDb()
        {
            if (provider.IsInMemory())
            {
                provider.EnsureDeleted();
            }
        }

        #region EditNews
        [Test]
        public void ShouldThrowExceptionWhenNewsForEditDoesNotExist()
        {
            Assert.Throws<Exception>(() => repository.EditNews(
                new DbNews() { Id = Guid.Empty }));
        }

        [Test]
        public void ShouldEditNews()
        {
            provider.MakeEntityDetached(dbNews);
            repository.EditNews(dbNewsRequest);

            var resultNews = provider.News
                .FirstOrDefaultAsync(x => x.Id == dbNewsRequest.Id)
                .Result;

            SerializerAssert.AreEqual(dbNewsRequest, resultNews);
        }
        #endregion
    }
}
