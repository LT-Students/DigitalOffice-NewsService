using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
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
        private DbNews dbNewsToAdd;

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

            dbNewsToAdd = new DbNews
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

        #region GetNews
        [Test]
        public void ShouldThrowExceptionWhenNewsForEditDoesNotExist()
        {
            Assert.Throws<NotFoundException>(() => repository.GetNews (Guid.Empty));
        }

        [Test]
        public void ShouldGetNews()
        {
            var recievedNews = repository.GetNews(dbNews.Id);
            var sameNews = provider.News
                .FirstOrDefaultAsync(x => x.Id == dbNews.Id)
                .Result;

            SerializerAssert.AreEqual(recievedNews, sameNews);
            SerializerAssert.AreEqual(dbNews, sameNews);

        }
        #endregion

        #region CreateNews
        [Test]
        public void ShouldReturnMatchingIdAndCreateNews()
        {
            var guidOfNews = repository.CreateNews(dbNewsToAdd);

            Assert.AreEqual(dbNewsToAdd.Id, guidOfNews);
            Assert.NotNull(provider.News.Find(dbNewsToAdd.Id));
        }
        #endregion

        #region EditNews
        [Test]
        public void ShouldThrowExceptionWhenNewsDoesNotExist()
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
