using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.NewsService.Business.UnitTests
{
    public class EditNewsCommandTests
    {
        private Mock<INewsRepository> _repositoryMock;
        private Mock<IPatchNewsMapper> _mapperMock;
        private Mock<IPatchNewsValidator> _validatorMock;

        private JsonPatchDocument<EditNewsRequest> _goodEditNewsRequest;
        private JsonPatchDocument<EditNewsRequest> _badEditNewsRequest;
        private JsonPatchDocument<DbNews> _dbNews;

        private Guid _goodNewsId = Guid.NewGuid();
        private Guid _badNewsId = Guid.NewGuid();

        private IEditNewsCommand _command;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _goodEditNewsRequest = new JsonPatchDocument<EditNewsRequest>();
            _badEditNewsRequest = null;
            _dbNews = new JsonPatchDocument<DbNews>();
        }

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<INewsRepository>();
            _repositoryMock
                .Setup(x => x.EditNews(_goodNewsId, It.IsAny<JsonPatchDocument<DbNews>>()))
                .Returns(true);
            _repositoryMock
                .Setup(x => x.EditNews(_badNewsId, It.IsAny<JsonPatchDocument<DbNews>>()))
                .Throws(new NotFoundException());

            _mapperMock = new Mock<IPatchNewsMapper>();
            _mapperMock
                .Setup(x => x.Map(_goodEditNewsRequest))
                .Returns(_dbNews);
            _mapperMock
                .Setup(x => x.Map(_badEditNewsRequest))
                .Throws(new ArgumentNullException());

            _validatorMock = new Mock<IPatchNewsValidator>();
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            _command = new EditNewsCommand(_repositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
        }

        [Test]
        public void Success()
        {
            SerializerAssert.AreEqual(true, _command.Execute(_goodNewsId, _goodEditNewsRequest));
        }

        [Test]
        public void BadValidator()
        {
        }

        [Test]
        public void MapperException()
        {
            Assert.Throws<ArgumentNullException>(() => _command.Execute(It.IsAny<Guid>(), _badEditNewsRequest));
        }

        [Test]
        public void RepositoryException()
        {
            Assert.Throws<NotFoundException> (() => _command.Execute(_badNewsId, _goodEditNewsRequest));
        }

    }
}
