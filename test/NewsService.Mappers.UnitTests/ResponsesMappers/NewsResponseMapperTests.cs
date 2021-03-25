using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.NewsService.Mappers.UnitTests.ResponsesMappers
{
    internal class NewsResponseMapperTests
    {
        private NewsResponseMapper newsResponseMapper;

        private const string Content = "smth";
        private const string AuthorName = "Ivan";
        private const string Subject = "Example";
        private const bool IsActive = true;

        private Guid newsId;
        private Guid authorId;
        private Guid senderId;
        private DateTime createdAt;

        private DbNews dbNews;

        [SetUp]
        public void SetUp()
        {
            newsResponseMapper = new NewsResponseMapper();

            newsId = Guid.NewGuid();
            authorId = Guid.NewGuid();
            senderId = Guid.NewGuid();
            createdAt = DateTime.Now;

            dbNews = new DbNews
            {
                Content = Content,
                AuthorName = AuthorName,
                Subject = Subject,
                IsActive = IsActive,
                Id = newsId,
                AuthorId = authorId,
                SenderId = senderId,
                CreatedAt = createdAt
            };
        }

        [Test]
        public void ShouldThrowBadRequestExceptionWhenDbNewsIsNull()
        {
            Assert.Throws<BadRequestException>(() => newsResponseMapper.Map(null));
        }

        [Test]
        public void ShouldReturnNewsModelWhenMappingValidDbNews()
        {
            var resultNewsModel = newsResponseMapper.Map(dbNews);

            Assert.IsNotNull(resultNewsModel);
            Assert.IsInstanceOf<News>(resultNewsModel);

            Assert.AreEqual(newsId, resultNewsModel.Id);
            Assert.AreEqual(AuthorName, resultNewsModel.AuthorName);
            Assert.AreEqual(Content, resultNewsModel.Content);
            Assert.AreEqual(Subject, resultNewsModel.Subject);
            Assert.AreEqual(IsActive, resultNewsModel.IsActive);
            Assert.AreEqual(authorId, resultNewsModel.AuthorId);
            Assert.AreEqual(senderId, resultNewsModel.SenderId);
            Assert.AreEqual(createdAt, resultNewsModel.CreatedAt);
        }
    }
}
