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
    public class CreateNewsCommandTests
    {
        private Mock<INewsMapper> _mapperMock;
        private Mock<INewsRepository> _repositoryMock;
        private Mock<INewsValidator> _validatorMock;

        private ICreateNewsCommand command;
        private News request;
        private DbNews createdNews;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            request = new News
            {
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "AuthorName",
                AuthorId = Guid.NewGuid(),
                SenderId = Guid.NewGuid()
            };

            createdNews = new DbNews
            {
                Id = Guid.NewGuid(),
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "AuthorName",
                AuthorId = request.AuthorId,
                SenderId = request.SenderId,
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

            command = new CreateNewsCommand(_repositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
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

            Assert.Throws<ValidationException>(() => command.Execute(request));
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

            Assert.Throws<Exception>(() => command.Execute(request));
            _repositoryMock.Verify(repository => repository.EditNews(It.IsAny<DbNews>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowingException()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            _mapperMock
                .Setup(x => x.Map(It.IsAny<News>()))
                .Returns(createdNews);

            _repositoryMock
                .Setup(x => x.CreateNews(It.IsAny<DbNews>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(request));
        }

        [Test]
        public void ShouldReturnIdFromRepositoryWhenNewsCreated()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            _mapperMock
                .Setup(x => x.Map(It.IsAny<News>()))
                .Returns(createdNews);

            _repositoryMock
                .Setup(x => x.CreateNews(It.IsAny<DbNews>()))
                .Returns(createdNews.Id);

            Assert.AreEqual(createdNews.Id, command.Execute(request));
            _repositoryMock.Verify(repository => repository.CreateNews(It.IsAny<DbNews>()), Times.Once);
        }
    }
}
