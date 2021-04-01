using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.NewsService.Business.UnitTests
{
    class FindNewsCommandTests
    {
        private Mock<INewsRepository> _goodRepository;
        private Mock<INewsRepository> _repositoryException;
        private Mock<INewsResponseMapper> _GoodMapper;
        private Mock<INewsResponseMapper> _mapperException;

        [SetUp]
        public void SetUp()
        {
            _repositoryException = new Mock<INewsRepository>();
            _repositoryException
                .Setup(x => x.FindNews(It.IsAny<FindNewsParams>()))
                .Throws(new Exception());

            _mapperException = new Mock<INewsResponseMapper>();
            _mapperException
                .Setup(x => x.Map(It.IsAny<DbNews>()))
                .Throws(new Exception());

        }

        [Test]
        public void RepositoryException()
        {


        }
    }
}
