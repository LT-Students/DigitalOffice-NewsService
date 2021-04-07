using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using NUnit.Framework;
using System;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models;

namespace LT.DigitalOffice.NewsService.Mappers.UnitTests.ModelMappers
{
    class NewsMapperTests
    {
        private INewsMapper mapper;
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
                Pseudonym = "AuthorName",
                AuthorId = Guid.NewGuid(),
                SenderId = Guid.NewGuid()
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
        #endregion
    }
}
