/*using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Business.UnitTests
{
    public class EditNewsCommandTests
    {
        private Mock<INewsMapper> mapperMock;
        private Mock<INewsRepository> repositoryMock;
        private Mock<IValidator<News>> validatorMock;

        private IEditNewsCommand command;
        private News request;
        private DbNews dbNews;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            request = new News
            {
                Id = Guid.NewGuid(),
                Content = "Content111",
                Subject = "Subject111",
                Pseudonym = "AuthorName111",
                AuthorId = Guid.NewGuid(),
                SenderId = Guid.NewGuid()
            };

            dbNews = new DbNews
            {
                Id = (Guid)request.Id,
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
            mapperMock = new Mock<INewsMapper>();
            repositoryMock = new Mock<INewsRepository>();
            validatorMock = new Mock<IValidator<News>>();

            command = new EditNewsCommand(repositoryMock.Object, mapperMock.Object, validatorMock.Object);
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
        public void ShouldThrowExceptionWhenRepositoryThrowException()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            mapperMock
                .Setup(x => x.Map(It.IsAny<News>()))
                .Returns(dbNews);

            repositoryMock
                .Setup(x => x.EditNews(It.IsAny<DbNews>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(request));
        }

        [Test]
        public void ShouldEditNews()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult());

            mapperMock
                .Setup(x => x.Map(It.IsAny<News>()))
                .Returns(dbNews);

            repositoryMock
                .Setup(x => x.EditNews(It.IsAny<DbNews>()));

            Assert.DoesNotThrow(() => command.Execute(request));
            repositoryMock.Verify(repository => repository.EditNews(It.IsAny<DbNews>()), Times.Once);
        }
    }
}*/
