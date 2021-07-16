using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Business.UnitTests
{
    public class CreateNewsCommandTests
    {
        private Mock<IDbNewsMapper> _mapperMock;
        private Mock<INewsRepository> _repositoryMock;
        private Mock<INewsValidator> _validatorMock;
        private Mock<IAccessValidator> _accessValidatorMock;

        private ICreateNewsCommand _command;
        private News _request;
        private DbNews _createdNews;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _request = new News
            {
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "AuthorName",
                AuthorId = Guid.NewGuid(),
                SenderId = Guid.NewGuid()
            };

            _createdNews = new DbNews
            {
                Id = Guid.NewGuid(),
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "AuthorName",
                AuthorId = _request.AuthorId,
                SenderId = _request.SenderId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IDbNewsMapper>();
            _repositoryMock = new Mock<INewsRepository>();
            _validatorMock = new Mock<INewsValidator>();
            _accessValidatorMock = new Mock<IAccessValidator>();

            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(true);

            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemoveNews))
                .Returns(true);

            _command = new CreateNewsCommand(
                _repositoryMock.Object,
                _mapperMock.Object,
                _validatorMock.Object,
                _accessValidatorMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenNotEnoughRights()
        {
            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemoveNews))
                .Returns(false);

            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(_request));
            _repositoryMock.Verify(x => x.CreateNews(It.IsAny<DbNews>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenValidatorThrowException()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new ValidationFailure("error", "something", null)
                    }));

            Assert.Throws<ValidationException>(() => _command.Execute(_request));
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowException()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            _mapperMock
                .Setup(x => x.Map(It.IsAny<News>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_request));
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowingException()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            _mapperMock
                .Setup(x => x.Map(It.IsAny<News>()))
                .Returns(_createdNews);

            _repositoryMock
                .Setup(x => x.CreateNews(It.IsAny<DbNews>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_request));
        }

        [Test]
        public void ShouldReturnIdFromRepositoryWhenNewsCreated()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            _mapperMock
                .Setup(x => x.Map(It.IsAny<News>()))
                .Returns(_createdNews);

            _repositoryMock
                .Setup(x => x.CreateNews(It.IsAny<DbNews>()))
                .Returns(_createdNews.Id);

            Assert.AreEqual(_createdNews.Id, _command.Execute(_request));
            _repositoryMock.Verify(repository => repository.CreateNews(It.IsAny<DbNews>()), Times.Once);
        }
    }
}
