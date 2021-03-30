using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using NUnit.Framework;
using System;
using LT.DigitalOffice.NewsService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ModelMappers;

namespace LT.DigitalOffice.NewsService.Mappers.UnitTests.ModelMappers
{
    class NewsMapperTests
    {
        private INewsMapper mapper;
        private News newsRequestWithId;
        private News newsRequest;
        private DbNews expectedDbNews;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mapper = new NewsMapper();

            newsRequest = new News
            {
                Content = "Content",
                Subject = "Subject",
                AuthorName = "AuthorName",
                AuthorId = Guid.NewGuid(),
                SenderId = Guid.NewGuid()
            };

            newsRequestWithId = new News
            {
                Id = Guid.NewGuid(),
                Content = "Content",
                Subject = "Subject",
                AuthorName = "AuthorName",
                AuthorId = newsRequest.AuthorId,
                SenderId = newsRequest.SenderId,
            };

            expectedDbNews = new DbNews
            {
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "AuthorName",
                AuthorId = newsRequest.AuthorId,
                SenderId = newsRequest.SenderId,
                IsActive = true
            };

        }

        #region NewsRequest to DbNews
        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => mapper.Map(null));
        }

        [Test]
        public void ShouldReturnRightModelWhenRequestIsMapped()
        {
            var dbNews = mapper.Map(newsRequest);
            expectedDbNews.Id = dbNews.Id;
            expectedDbNews.CreatedAt = dbNews.CreatedAt;

            Assert.IsInstanceOf<Guid>(dbNews.Id);
            SerializerAssert.AreEqual(expectedDbNews, dbNews);
        }

        [Test]
        public void ShouldReturnRightModelWhenRequestWithIdIsMapped()
        {
            var dbNews = mapper.Map(newsRequestWithId);

            expectedDbNews.Id = dbNews.Id;
            expectedDbNews.CreatedAt = dbNews.CreatedAt;

            Assert.IsInstanceOf<Guid>(dbNews.Id);
            SerializerAssert.AreEqual(expectedDbNews, dbNews);
        }
        #endregion
    }
}
