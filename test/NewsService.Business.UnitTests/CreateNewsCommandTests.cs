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
    public class CreateNewsCommandTests
    {
        private Mock<IMapper<CreateNewsRequest, DbNews>> mapperMock;
        private Mock<INewsRepository> repositoryMock;

        private ICreateNewsCommand command;
        private CreateNewsRequest request;
        private DbNews createdNews;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            request = new CreateNewsRequest
            {
                Content = "Content",
                Subject = "Subject",
                AuthorName = "AuthorName",
                AuthorId = Guid.NewGuid(),
                SenderId = Guid.NewGuid()
            };

            createdNews = new DbNews
            {
                Id = Guid.NewGuid(),
                Content = "Content",
                Subject = "Subject",
                AuthorName = "AuthorName",
                AuthorId = request.AuthorId,
                SenderId = request.SenderId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper<CreateNewsRequest, DbNews>>();
            repositoryMock = new Mock<INewsRepository>();

            command = new CreateNewsCommand(repositoryMock.Object, mapperMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowingException()
        {
            mapperMock
                .Setup(x => x.Map(It.IsAny<CreateNewsRequest>()))
                .Returns(createdNews);

            repositoryMock
                .Setup(x => x.CreateNews(It.IsAny<DbNews>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(request));
        }

        [Test]
        public void ShouldReturnIdFromRepositoryWhenNewsCreated()
        {
            mapperMock
                .Setup(x => x.Map(It.IsAny<CreateNewsRequest>()))
                .Returns(createdNews);

            repositoryMock
                .Setup(x => x.CreateNews(It.IsAny<DbNews>()))
                .Returns(createdNews.Id);

            Assert.AreEqual(createdNews.Id, command.Execute(request));
            repositoryMock.Verify(repository => repository.CreateNews(It.IsAny<DbNews>()), Times.Once);
        }
    }
}
