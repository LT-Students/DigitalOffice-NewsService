using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.NewsService.Business.UnitTests
{
    public class GetNewsCommandTests
    {
        private Mock<IMapper<DbNews, News>> mapperMock;
        private Mock<INewsRepository> repositoryMock;

        private IGetNewsCommand command;
        private News response;
        private DbNews dbNews;

        [OneTimeSetUp]
        public void OneTimeSetUp()
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
            response = new News
            {
                Id = dbNews.Id,
                Content = dbNews.Content,
                Subject = dbNews.Subject,
                AuthorName = dbNews.AuthorName,
                AuthorId = dbNews.AuthorId,
                SenderId = dbNews.SenderId,
                CreatedAt = dbNews.CreatedAt,
                IsActive = dbNews.IsActive
            };
        }

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper<DbNews, News>>();
            repositoryMock = new Mock<INewsRepository>();

            command = new GetNewsCommand(repositoryMock.Object, mapperMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowingException()
        {
            repositoryMock
                .Setup(x => x.GetNews(It.IsAny<Guid>()))
                .Throws(new Exception());

            mapperMock
                .Setup(x => x.Map(It.IsAny<DbNews>()))
                .Returns(response);

            Assert.Throws<Exception>(() => command.Execute(dbNews.Id));
            repositoryMock.Verify(repository => repository.GetNews(It.IsAny<Guid>()), Times.Once);
            mapperMock.Verify(mapper => mapper.Map(It.IsAny<DbNews>()), Times.Never);
        }

        [Test]
        public void ShouldReturnNewsFromRepositoryWhenNewsExists()
        {
            repositoryMock
                .Setup(x => x.GetNews(It.IsAny<Guid>()))
                .Returns(dbNews);

            mapperMock
                .Setup(x => x.Map(It.IsAny<DbNews>()))
                .Returns(response);

            Assert.AreEqual(response, command.Execute(dbNews.Id));
            repositoryMock.Verify(repository => repository.GetNews(It.IsAny<Guid>()), Times.Once);
            mapperMock.Verify(mapper => mapper.Map(It.IsAny<DbNews>()), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowingException()
        {
            repositoryMock
                .Setup(x => x.GetNews(It.IsAny<Guid>()))
                .Returns(dbNews);

            mapperMock
                .Setup(x => x.Map(It.IsAny<DbNews>()))
                .Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => command.Execute(dbNews.Id));
            repositoryMock.Verify(repository => repository.GetNews(It.IsAny<Guid>()), Times.Once);
            mapperMock.Verify(mapper => mapper.Map(It.IsAny<DbNews>()), Times.Once);
        }
    }
}
