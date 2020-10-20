using LT.DigitalOffice.Kernel.UnitTestLibrary;
using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.NewsService.Mappers.UnitTests
{
    class NewsMapperTests
    {
        private IMapper<NewsRequest, DbNews> requestMapper;
        private NewsRequest newsRequestWithId;
        private NewsRequest newsRequest;
        private DbNews dbNews;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            requestMapper = new NewsMapper();
            var id = Guid.NewGuid();
            var content = "Content";
            var subject = "Subject";
            var authorName = "AuthorName";
            var authorId = Guid.NewGuid();
            var senderId = Guid.NewGuid();

            newsRequest = new NewsRequest
            {
                Content = content,
                Subject = subject,
                AuthorName = authorName,
                AuthorId = authorId,
                SenderId = senderId
            };

            newsRequestWithId = new NewsRequest
            {
                Id = id,
                Content = content,
                Subject = subject,
                AuthorName = authorName,
                AuthorId = authorId,
                SenderId = senderId,
            };

            dbNews = new DbNews
            {
                Content = content,
                Subject = subject,
                AuthorName = authorName,
                AuthorId = authorId,
                SenderId = senderId,
                IsActive = true
            };

        }

        #region NewsRequest to DbNews
        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => requestMapper.Map(null));
        }

        [Test]
        public void ShouldReturnRightModelWhenRequestIsMapped()
        {
            var dbNews = requestMapper.Map(newsRequest);
            this.dbNews.Id = dbNews.Id;
            this.dbNews.CreatedAt = dbNews.CreatedAt;

            Assert.IsInstanceOf<Guid>(dbNews.Id);
            SerializerAssert.AreEqual(this.dbNews, dbNews);
        }

        [Test]
        public void ShouldReturnRightModelWhenRequestWithIdIsMapped()
        {
            var dbNews = requestMapper.Map(newsRequestWithId);

            this.dbNews.Id = dbNews.Id;
            this.dbNews.CreatedAt = dbNews.CreatedAt;

            Assert.IsInstanceOf<Guid>(dbNews.Id);
            SerializerAssert.AreEqual(this.dbNews, dbNews);
        }
        #endregion

        #region DbNews to News
        [Test]
        public void ShouldThrowArgumentNullExceptionWhenDbNewsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => requestMapper.Map(null));
        }

        [Test]
        public void ShouldReturnRightModelWhenDbModelIsMapped()
        {
            var news = requestMapper.Map(dbNews);

            Assert.IsInstanceOf<Guid>(dbNews.Id);
            SerializerAssert.AreEqual(this.dbNews, dbNews);
        }

        #endregion
    }
}
