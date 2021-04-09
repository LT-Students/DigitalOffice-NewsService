using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Data.UnitTests
{
    class NewsRepositoryTests
    {
        private IDataProvider _provider;
        private INewsRepository _repository;

        private DbNews _dbNews;
        private DbNews _dbNewsToAdd;

        private Guid _firstUserId = Guid.NewGuid();
        private Guid _secondUserId = Guid.NewGuid();

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
            _dbNews = new DbNews
            {
                Id = Guid.NewGuid(),
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "Pseudonym",
                AuthorId = _firstUserId,
                SenderId = _firstUserId,
                CreatedAt = DateTime.UtcNow,
                DepartmentId = Guid.NewGuid(),
                IsActive = true
            };

            _dbNewsToAdd = new DbNews
            {
                Id = Guid.NewGuid(),
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "Pseudonym",
                AuthorId = _firstUserId,
                SenderId = _secondUserId,
                CreatedAt = DateTime.UtcNow,
                DepartmentId = Guid.NewGuid(),
                IsActive = true
            };

            _provider.News.Add(_dbNews);
            _provider.Save();
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
        }
        #endregion

        #region CreateNews
        [Test]
        public void ShouldReturnMatchingIdAndCreateNews()
        {
            SerializerAssert.AreEqual(_dbNewsToAdd.Id, _repository.CreateNews(_dbNewsToAdd));
            Assert.NotNull(_provider.News.Find(_dbNewsToAdd.Id));
        }
        #endregion

        #region FindNews
        [Test]
        public void ExceptionNullFindNewsParams()
        {
            Assert.Throws<ArgumentNullException>(() => _repository.FindNews(null));
        }

        [Test]
        public void FinedNews()
        {
            _provider.MakeEntityDetached(_dbNews);

            SerializerAssert.AreEqual(
                new List<DbNews> { _dbNews },
                _repository.FindNews(new FindNewsParams { AuthorId = _firstUserId}));

            SerializerAssert.AreEqual(
                new List<DbNews> { _dbNews },
                _repository.FindNews(new FindNewsParams { DepartmentId = _dbNews.DepartmentId }));

            SerializerAssert.AreEqual(
                new List<DbNews> { _dbNews },
                _repository.FindNews(new FindNewsParams { Pseudonym = _dbNews.Pseudonym }));

            SerializerAssert.AreEqual(
                new List<DbNews> { _dbNews },
                _repository.FindNews(new FindNewsParams { Subject = _dbNews.Subject }));

            SerializerAssert.AreEqual(
                new List<DbNews> { _dbNews },
                _repository.FindNews(new FindNewsParams
                {
                    AuthorId = _firstUserId,
                    DepartmentId = _dbNews.DepartmentId,
                    Pseudonym = _dbNews.Pseudonym,
                    Subject = _dbNews.Subject
                }));

            SerializerAssert.AreEqual(
                new List<DbNews> { _dbNews },
                _repository.FindNews(new FindNewsParams {}));
        }
        #endregion

        #region GetNews
        [Test]
        public void ShouldThrowExceptionWhenThereNoNewsInDatabaseWithSuchId()
        {
            Assert.Throws<NotFoundException>(() => _repository.GetNewsInfoById(Guid.NewGuid()));
        }

        [Test]
        public void ShouldReturnNewsInfoWhenGettingFileById()
        {
            var result = _repository.GetNewsInfoById(_dbNews.Id);

            var expected = new DbNews
            {
                Id = _dbNews.Id,
                Content = _dbNews.Content,
                Subject = _dbNews.Subject,
                Pseudonym = _dbNews.Pseudonym,
                AuthorId = _dbNews.AuthorId,
                SenderId = _dbNews.SenderId,
                CreatedAt = _dbNews.CreatedAt,
                DepartmentId = _dbNews.DepartmentId,
                IsActive = _dbNews.IsActive
            };

            SerializerAssert.AreEqual(expected, result);
        }
        #endregion
    }
}
