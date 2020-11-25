using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Business.UnitTests
{
    public class CreateNewsCommandTests
    {
        private Mock<INewsMapper> mapperMock;
        private Mock<INewsRepository> repositoryMock;
        private Mock<IValidator<News>> validatorMock;

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
                AuthorName = "AuthorName",
                AuthorId = Guid.NewGuid(),
                SenderId = Guid.NewGuid()
            };

            createdNews = new DbNews
            {
                Id = Guid.NewGuid(),
                Content = "Content",
                Subject = "Subject",
                AuthorName = "AuthorName",
                AuthorId = request.AuthorId,
                SenderId = request.SenderId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<INewsMapper>();
            repositoryMock = new Mock<INewsRepository>();
            validatorMock = new Mock<IValidator<News>>();

            command = new CreateNewsCommand(repositoryMock.Object, mapperMock.Object, validatorMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenValidatorThrowException()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new ValidationFailure("error", "something", null)
                    }));

            Assert.Throws<ValidationException>(() => command.Execute(request));
            repositoryMock.Verify(repository => repository.EditNews(It.IsAny<DbNews>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowException()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            mapperMock
                .Setup(x => x.Map(It.IsAny<News>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(request));
            repositoryMock.Verify(repository => repository.EditNews(It.IsAny<DbNews>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowingException()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            mapperMock
                .Setup(x => x.Map(It.IsAny<News>()))
                .Returns(createdNews);

            repositoryMock
                .Setup(x => x.CreateNews(It.IsAny<DbNews>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(request));
        }

        [Test]
        public void ShouldReturnIdFromRepositoryWhenNewsCreated()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            mapperMock
                .Setup(x => x.Map(It.IsAny<News>()))
                .Returns(createdNews);

            repositoryMock
                .Setup(x => x.CreateNews(It.IsAny<DbNews>()))
                .Returns(createdNews.Id);

            Assert.AreEqual(createdNews.Id, command.Execute(request));
            repositoryMock.Verify(repository => repository.CreateNews(It.IsAny<DbNews>()), Times.Once);
        }
    }
}
