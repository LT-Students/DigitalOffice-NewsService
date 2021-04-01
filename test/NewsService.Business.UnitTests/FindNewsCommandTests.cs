using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ModelMappers.Interfaces;
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
        private Mock<INewsMapper> _GoodMapper;
        private Exception _exception;

        [SetUp]
        public void SetUp()
        {
            _exception = new Exception();

        }

        [Test]

        public void RepositoryException()
        {
            _repositoryException = new Mock<INewsRepository>();
            _repositoryException
                .Setup(x => x.FindNews(It.IsAny<FindNewsParams>())
                //.Returns(_exception)

        }
    }
}
