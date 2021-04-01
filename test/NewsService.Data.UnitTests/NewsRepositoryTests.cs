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
        private IDataProvider _provider;
        private INewsRepository _repository;

        private DbNews dbNewsRequest;
        private DbNews dbNews;
        private DbNews dbNewsToAdd;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<NewsServiceDbContext>()
                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                   .Options;
            _provider = new NewsServiceDbContext(dbOptions);

            _repository = new NewsRepository(_provider);
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

            _provider.News.Add(dbNews);
            _provider.Save();

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
            if (_provider.IsInMemory())
            {
                _provider.EnsureDeleted();
            }
        }

        #region EditNews
        [Test]
        public void ShouldThrowExceptionWhenNewsForEditDoesNotExist()
        {
            Assert.Throws<Exception>(() => _repository.EditNews(
                new DbNews() { Id = Guid.Empty }));
        }

        [Test]
        public void ShouldEditNews()
        {
            _provider.MakeEntityDetached(dbNews);
            _repository.EditNews(dbNewsRequest);

            var resultNews = _provider.News
                .FirstOrDefaultAsync(x => x.Id == dbNewsRequest.Id)
                .Result;

            SerializerAssert.AreEqual(dbNewsRequest, resultNews);
        }
        #endregion

        #region CreateNews
        [Test]
        public void ShouldReturnMatchingIdAndCreateNews()
        {
            var guidOfNews = _repository.CreateNews(dbNewsToAdd);

            Assert.AreEqual(dbNewsToAdd.Id, guidOfNews);
            Assert.NotNull(_provider.News.Find(dbNewsToAdd.Id));
        }
        #endregion

        #region FindNews
        [Test]
        public void ExceptionNullFindNewsParams()
        {
            Assert.Throws<Exception>(() => _repository.FindNews(null));
        }
        #endregion
    }
}
