using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interface;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.ModelResponse;
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
        private NewsResponse newsresponse;
        private DbNews dbNews;
        private DbNews badNews;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<INewsRepository>();
            mapperMock = new Mock<INewsResponseMapper>();
            getNewsInfoByIdCommand = new GetNewsByIdCommand(repositoryMock.Object, mapperMock.Object);

            newsId = Guid.NewGuid();
            newsresponse = new NewsResponse { Id = newsId };
            dbNews = new DbNews { Id = newsId };
            badNews = new DbNews();

            repositoryMock.Setup(repository => repository.GetNewsInfoById(newsId)).Returns(dbNews).Verifiable();
            mapperMock.Setup(mapper => mapper.Map(dbNews)).Returns(newsresponse).Verifiable();

            mapperMock.Setup(mapper => mapper.Map(badNews)).Throws<Exception>().Verifiable();
        }

        [Test]
        public void ShouldReturnModelOfNews()
        {
            SerializerAssert.AreEqual(newsresponse, getNewsInfoByIdCommand.Execute(newsId));
            repositoryMock.Verify();
            mapperMock.Verify(mapper => mapper.Map(It.IsAny<DbNews>()), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsIt()
        {
            mapperMock.Setup(x => x.Map(It.IsAny<DbNews>())).Throws(new BadRequestException());

            Assert.Throws<BadRequestException>(() => getNewsInfoByIdCommand.Execute(newsId));
            mapperMock.Verify(mapper => mapper.Map(It.IsAny<DbNews>()), Times.Once);
            repositoryMock.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsIt()
        {
            repositoryMock.Setup(repository => repository.GetNewsInfoById(newsId)).Throws<NotFoundException>().Verifiable();

            Assert.Throws<NotFoundException>(() => getNewsInfoByIdCommand.Execute(newsId));
            repositoryMock.Verify();
            mapperMock.Verify(mapper => mapper.Map(It.IsAny<DbNews>()), Times.Never);
        }
    }
}
