using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Data.UnitTests
{
    class NewsRepositoryTests
    {
        private IDataProvider _provider;
        private INewsRepository _repository;
        private Mock<IHttpContextAccessor> _httpContext;

        private Guid _userId = Guid.NewGuid();
        private DbNews _dbNews;
        private DbNews _dbNewsToAdd;
        private JsonPatchDocument<DbNews> _editDbNews;
        private JsonPatchDocument<DbNews> _editDbNewsSubject;
        private JsonPatchDocument<DbNews> _editDbNewsContent;
        private JsonPatchDocument<DbNews> _editDbNewsIsActive;

        private Guid _firstUserId = Guid.NewGuid();
        private Guid _secondUserId = Guid.NewGuid();
        private string _firstValue = "firstValue";
        private string _secondValue = "secondValue";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _httpContext = new();
            IDictionary<object, object> _items = new Dictionary<object, object>();
            _items.Add("UserId", _userId);

            _httpContext
                .Setup(x => x.HttpContext.Items)
                .Returns(_items);

            var dbOptions = new DbContextOptionsBuilder<NewsServiceDbContext>()
                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                   .Options;

            _provider = new NewsServiceDbContext(dbOptions);

            _repository = new NewsRepository(_provider, _httpContext.Object);
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
                DepartmentId = Guid.NewGuid(),
                IsActive = true,
                CreatedBy = Guid.NewGuid(),
                CreatedAtUtc = DateTime.UtcNow
            };

            _editDbNews = new JsonPatchDocument<DbNews>();
            _editDbNews.Operations.Add(new Operation<DbNews>("replace", $"/{nameof(DbNews.Subject)}", "", _firstValue));
            _editDbNews.Operations.Add(new Operation<DbNews>("replace", $"/{nameof(DbNews.Content)}", "", _firstValue));
            _editDbNews.Operations.Add(new Operation<DbNews>("replace", $"/{nameof(DbNews.IsActive)}", "", "false"));

            _editDbNewsSubject = new JsonPatchDocument<DbNews>();
            _editDbNewsSubject.Operations.Add(new Operation<DbNews>("replace", $"/{nameof(DbNews.Subject)}", "", _secondValue));

            _editDbNewsContent = new JsonPatchDocument<DbNews>();
            _editDbNewsContent.Operations.Add(new Operation<DbNews>("replace", $"/{nameof(DbNews.Content)}", "", _secondValue));

            _editDbNewsIsActive = new JsonPatchDocument<DbNews>();
            _editDbNewsIsActive.Operations.Add(new Operation<DbNews>("replace", $"/{nameof(DbNews.IsActive)}", "", "true"));

            _dbNewsToAdd = new DbNews
            {
                Id = Guid.NewGuid(),
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "Pseudonym",
                AuthorId = _firstUserId,
                SenderId = _secondUserId,
                DepartmentId = Guid.NewGuid(),
                IsActive = true,
                CreatedBy = Guid.NewGuid(),
                CreatedAtUtc = DateTime.UtcNow
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
        public void SuccessfullyEditNewsWithAnyСombinationRequestProperties()
        {
            var editNews = _repository.EditNews(_dbNews.Id, _editDbNews);
            SerializerAssert.AreEqual(true, editNews);
            var check = _provider.News.Find(_dbNews.Id);
            Assert.AreEqual(_firstValue, check.Subject);
            Assert.AreEqual(_firstValue, check.Content);
            Assert.AreEqual(false, check.IsActive);

            SerializerAssert.AreEqual(true, _repository.EditNews(_dbNews.Id, _editDbNewsSubject));
            Assert.AreEqual(_secondValue, _provider.News.Find(_dbNews.Id).Subject);

            SerializerAssert.AreEqual(true, _repository.EditNews(_dbNews.Id, _editDbNewsContent));
            Assert.AreEqual(_secondValue, _provider.News.Find(_dbNews.Id).Content);

            SerializerAssert.AreEqual(true, _repository.EditNews(_dbNews.Id, _editDbNewsIsActive));
            Assert.AreEqual(true, _provider.News.Find(_dbNews.Id).IsActive);
        }

        [Test]
        public void ExceptionIfNewsNotFound()
        {
            Assert.Throws<NotFoundException>(() => _repository.EditNews(Guid.NewGuid(), _editDbNews));
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
                _repository.FindNews(new FindNewsFilter { AuthorId = _firstUserId }));

            SerializerAssert.AreEqual(
                new List<DbNews> { _dbNews },
                _repository.FindNews(new FindNewsFilter { DepartmentId = _dbNews.DepartmentId }));

            SerializerAssert.AreEqual(
                new List<DbNews> { _dbNews },
                _repository.FindNews(new FindNewsFilter { Pseudonym = _dbNews.Pseudonym }));

            SerializerAssert.AreEqual(
                new List<DbNews> { _dbNews },
                _repository.FindNews(new FindNewsFilter { Subject = _dbNews.Subject }));

            SerializerAssert.AreEqual(
                new List<DbNews> { _dbNews },
                _repository.FindNews(new FindNewsFilter
                {
                    AuthorId = _firstUserId,
                    DepartmentId = _dbNews.DepartmentId,
                    Pseudonym = _dbNews.Pseudonym,
                    Subject = _dbNews.Subject
                }));

            SerializerAssert.AreEqual(
                new List<DbNews> { _dbNews },
                _repository.FindNews(new FindNewsFilter {}));
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
                DepartmentId = _dbNews.DepartmentId,
                IsActive = _dbNews.IsActive,
                CreatedBy = _dbNews.CreatedBy,
                CreatedAtUtc = _dbNews.CreatedAtUtc
            };

            SerializerAssert.AreEqual(expected, result);
        }

        #endregion
    }
}
