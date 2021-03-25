using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interface;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.NewsService.Business.UnitTests
{
    public class GetNewsByIdCommandTests
    {
        private IGetNewsByIdCommand getNewsInfoByIdCommand;
        private Mock<INewsRepository> repositoryMock;
        private Mock<INewsResponseMapper> mapperMock;

        private Guid newsId;
        private News news;
        private DbNews dbNews;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<INewsRepository>();
            mapperMock = new Mock<INewsResponseMapper>();
            getNewsInfoByIdCommand = new GetNewsByIdCommand(repositoryMock.Object, mapperMock.Object);

            newsId = Guid.NewGuid();
            news = new News { Id = newsId };
            dbNews = new DbNews { Id = newsId };
        }

        [Test]
        public void ShouldReturnModelOfNews()
        {
            repositoryMock.Setup(repository => repository.GetNewsInfoById(newsId)).Returns(dbNews).Verifiable();
            mapperMock.Setup(mapper => mapper.Map(dbNews)).Returns(news).Verifiable();

            SerializerAssert.AreEqual(news, getNewsInfoByIdCommand.Execute(newsId));
            repositoryMock.Verify();
            mapperMock.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsException()
        {
            repositoryMock.Setup(repository => repository.GetNewsInfoById(It.IsAny<Guid>())).Returns(dbNews).Verifiable();
            mapperMock.Setup(mapper => mapper.Map(It.IsAny<DbNews>())).Throws<Exception>().Verifiable();

            Assert.Throws<Exception>(() => getNewsInfoByIdCommand.Execute(It.IsAny<Guid>()));
            mapperMock.Verify();
            repositoryMock.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsException()
        {
            repositoryMock.Setup(repository => repository.GetNewsInfoById(newsId)).Throws<NotFoundException>().Verifiable();
            mapperMock.Setup(mapper => mapper.Map(dbNews)).Returns(news);

            Assert.Throws<NotFoundException>(() => getNewsInfoByIdCommand.Execute(newsId));
            repositoryMock.Verify();
            mapperMock.Verify(mapper => mapper.Map(It.IsAny<DbNews>()), Times.Never);
        }
    }
}
