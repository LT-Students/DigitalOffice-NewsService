using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Business.UnitTests
{
    public class EditNewsCommandTests
    {
        private Mock<INewsRepository> _repositoryMock;
        private Mock<IPatchNewsMapper> _mapperMock;
        private Mock<IEditNewsValidator> _validatorMock;
        private Mock<IAccessValidator> _accessValidatorMock;
    private Mock<IHttpContextAccessor> _httpContextAccessor;

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

            _validatorMock = new Mock<IEditNewsValidator>();
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            _accessValidatorMock = new Mock<IAccessValidator>();

            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(true);

            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemoveNews))
                .Returns(true);

            _command = new EditNewsCommand(
                _repositoryMock.Object,
                _mapperMock.Object,
                _validatorMock.Object,
                _accessValidatorMock.Object,
                _httpContextAccessor.Object);
        }

/*        [Test]
        public void ShouldThrowExceptionWhenNotEnoughRights()
        {
            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemoveNews))
                .Returns(false);

            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(_goodNewsId, _goodEditNewsRequest));
            _repositoryMock.Verify(x => x.EditNews(It.IsAny<Guid>(), It.IsAny<JsonPatchDocument<DbNews>>()), Times.Never);
        }

        [Test]
        public void SuccessCommandTest()
        {
            SerializerAssert.AreEqual(true, _command.Execute(_goodNewsId, _goodEditNewsRequest));
        }

        [Test]
        public void BadValidatorTest()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                        {
                            new ValidationFailure("error", "something", null)
                        }));

            Assert.Throws<ValidationException>(() => _command.Execute(_goodNewsId, _goodEditNewsRequest));
        }

        [Test]
        public void MapperExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(() => _command.Execute(It.IsAny<Guid>(), _badEditNewsRequest));
        }

        [Test]
        public void RepositoryExceptionTest()
        {
            Assert.Throws<NotFoundException> (() => _command.Execute(_badNewsId, _goodEditNewsRequest));
        }
*/
    }
}
