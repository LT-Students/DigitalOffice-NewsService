using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Business.UnitTests
{
    class FindNewsCommandTests
    {
        private Mock<INewsRepository> _repositoryMock;
        private Mock<INewsResponseMapper> _mapperMock;

        private FindNewsFilter _goodFinedNewsFilter;
        private FindNewsFilter _badFinedNewsDilter;
        private FindNewsFilter _finedNewsFilterReturnsNullOfDbNews;

        private List<DbNews> _goodDbNewsList;
        private List<DbNews> _badDbNewsList;
        private List<NewsResponse> _newsResponse;

        private IFindNewsCommand _command;

        [SetUp]
        public void SetUp()
        {
            _goodFinedNewsFilter = new FindNewsFilter();
            _badFinedNewsDilter = new FindNewsFilter();
            _finedNewsFilterReturnsNullOfDbNews = new FindNewsFilter();

            var dbNews = new DbNews();
            var news = new NewsResponse();

            _goodDbNewsList = new List<DbNews> { dbNews, dbNews };
            _badDbNewsList = new List<DbNews> { null };
            _newsResponse = new List<NewsResponse> { news, news };

            _repositoryMock = new Mock<INewsRepository>();
            _repositoryMock
                .Setup(x => x.FindNews(_goodFinedNewsFilter))
                .Returns(_goodDbNewsList);
            _repositoryMock
                .Setup(x => x.FindNews(_finedNewsFilterReturnsNullOfDbNews))
                .Returns(_badDbNewsList);
            _repositoryMock
                .Setup(x => x.FindNews(_badFinedNewsDilter))
                .Throws(new Exception());
            _repositoryMock
                .Setup(x => x.FindNews(null))
                .Throws(new Exception());

            _mapperMock = new Mock<INewsResponseMapper>();
            _mapperMock
                .Setup(x => x.Map(dbNews))
                .Returns(news);
            _mapperMock
                .Setup(x => x.Map(null))
                .Throws(new Exception());

            _command = new FindNewsCommand(_repositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public void CommandTest()
        {
            SerializerAssert.AreEqual(_newsResponse, _command.Execute(_goodFinedNewsFilter));
        }

        [Test]
        public void MapperNullDbNewsTest()
        {
            Assert.Throws<Exception>(() => _command.Execute(_finedNewsFilterReturnsNullOfDbNews));
        }

        [Test]
        public void RepositoryBadFindNewsParamsTest()
        {
            Assert.Throws<Exception>(() => _command.Execute(_badFinedNewsDilter));
        }

        [Test]
        public void RepositoryNullFindNewsParamsTest()
        {
            Assert.Throws<Exception>(() => _command.Execute(null));
        }
    }
}
