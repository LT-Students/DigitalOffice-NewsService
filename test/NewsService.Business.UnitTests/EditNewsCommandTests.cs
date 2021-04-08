using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Business.UnitTests
{
    public class EditNewsCommandTests
    {
        private Mock<INewsMapper> _mapperMock;
        private Mock<INewsRepository> _repositoryMock;
        private Mock<INewsValidator> _validatorMock;

        private IEditNewsCommand _command;
        private News _request;
        private DbNews _dbNews;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _request = new News
            {
                Id = Guid.NewGuid(),
                Content = "Content111",
                Subject = "Subject111",
                Pseudonym = "AuthorName111",
                AuthorId = Guid.NewGuid(),
                SenderId = Guid.NewGuid()
            };

            _dbNews = new DbNews
            {
                Id = (Guid)_request.Id,
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
            _mapperMock = new Mock<INewsMapper>();
            _repositoryMock = new Mock<INewsRepository>();
            _validatorMock = new Mock<INewsValidator>();

            _command = new EditNewsCommand(_repositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
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
            _repositoryMock.Verify(repository => repository.EditNews(It.IsAny<DbNews>()), Times.Never);
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
            _repositoryMock.Verify(repository => repository.EditNews(It.IsAny<DbNews>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowException()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            _mapperMock
                .Setup(x => x.Map(It.IsAny<News>()))
                .Returns(_dbNews);

            _repositoryMock
                .Setup(x => x.EditNews(It.IsAny<DbNews>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_request));
        }

        [Test]
        public void ShouldEditNews()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            _mapperMock
                .Setup(x => x.Map(It.IsAny<News>()))
                .Returns(_dbNews);

            _repositoryMock
                .Setup(x => x.EditNews(It.IsAny<DbNews>()));

            Assert.DoesNotThrow(() => _command.Execute(_request));
            _repositoryMock.Verify(repository => repository.EditNews(It.IsAny<DbNews>()), Times.Once);
        }
    }
}
