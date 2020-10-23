using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.NewsService.Models.Dto;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.NewsService.Validation.UnitTests
{
    public class NewsValidatorTests
    {
        private IValidator<NewsRequest> validator;
        private NewsRequest request;

        [SetUp]
        public void SetUp()
        {
            validator = new NewsValidator();

            request = new NewsRequest
            {
                Subject = "Subject",
                AuthorId = Guid.NewGuid(),
                AuthorName = "Spartak Ryabtsev",
                Content = "Content",
                SenderId = Guid.NewGuid()
            };
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenSubjectIsEmpty()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Subject, "");
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenSubjectIsTooLong()
        {
            var subject = request.Subject.PadLeft(300);

            validator.ShouldHaveValidationErrorFor(x => x.Subject, subject);
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenAuthorIdIsEmpty()
        {
            validator.ShouldHaveValidationErrorFor(x => x.AuthorId, Guid.Empty);
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenAuthorNameIsEmpty()
        {
            validator.ShouldHaveValidationErrorFor(x => x.AuthorName, "");
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenAuthorNameIsTooLong()
        {
            var authorName = request.AuthorName.PadLeft(100);

            validator.ShouldHaveValidationErrorFor(x => x.AuthorName, authorName);
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenContentIsEmpty()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Subject, "");
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenSenderIdIsEmpty()
        {
            validator.ShouldHaveValidationErrorFor(x => x.SenderId, Guid.Empty);
        }

        [Test]
        public void ShouldNotHaveAnyValidationErrorsWhenRequestIsValid()
        {
            validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
        }
    }
}
