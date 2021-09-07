using System;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace LT.DigitalOffice.NewsService.Business.UnitTests
{
  public class GetNewsByIdCommandTests
  {
    private IGetNewsCommand _getNewsInfoByIdCommand;
    private Mock<INewsRepository> _repositoryMock;
    private Mock<INewsResponseMapper> _mapperMock;
    private Mock<IHttpContextAccessor> _httpContextAccessor;

    private Guid _newsId;
    private NewsResponse _newsresponse;
    private DbNews _dbNews;
    private DbNews _badNews;

    [SetUp]
    public void SetUp()
    {
      _repositoryMock = new Mock<INewsRepository>();
      _mapperMock = new Mock<INewsResponseMapper>();
      _getNewsInfoByIdCommand = new GetNewsCommand(_repositoryMock.Object, _mapperMock.Object, _httpContextAccessor.Object);

      _newsId = Guid.NewGuid();
      _newsresponse = new NewsResponse { Id = _newsId };
      _dbNews = new DbNews { Id = _newsId };
      _badNews = new DbNews();

      _repositoryMock
          .Setup(repository => repository.Get(_newsId))
          .Returns(_dbNews).Verifiable();
      _mapperMock
          .Setup(mapper => mapper.Map(_dbNews))
          .Returns(_newsresponse).Verifiable();

      _mapperMock
          .Setup(mapper => mapper.Map(_badNews))
          .Throws<Exception>().Verifiable();
    }

/*    [Test]
    public void ShouldReturnModelOfNews()
    {
      SerializerAssert.AreEqual(_newsresponse, _getNewsInfoByIdCommand.Execute(_newsId));
      _repositoryMock.Verify();
      _mapperMock
          .Verify(mapper => mapper.Map(It.IsAny<DbNews>()), Times.Once);
    }

    [Test]
    public void ShouldThrowExceptionWhenMapperThrowsIt()
    {
      _mapperMock
          .Setup(x => x.Map(It.IsAny<DbNews>()))
          .Throws(new BadRequestException());

      Assert.Throws<BadRequestException>(() => _getNewsInfoByIdCommand.Execute(_newsId));
      _mapperMock.Verify(mapper => mapper.Map(It.IsAny<DbNews>()), Times.Once);
      _repositoryMock.Verify();
    }

    [Test]
    public void ShouldThrowExceptionWhenRepositoryThrowsIt()
    {
      _repositoryMock
          .Setup(repository => repository.Get(_newsId))
          .Throws<NotFoundException>().Verifiable();

      Assert.Throws<NotFoundException>(() => _getNewsInfoByIdCommand.Execute(_newsId));
      _repositoryMock.Verify();
      _mapperMock
          .Verify(mapper => mapper.Map(It.IsAny<DbNews>()), Times.Never);
    }*/
  }
}
