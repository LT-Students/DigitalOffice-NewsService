/*using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.NewsService.Validation.UnitTests
{
  public class NewsValidatorTests
  {
    private INewsValidator validator;
    private News request;

    [SetUp]
    public void SetUp()
    {
      validator = new NewsValidator();

      request = new News
      {
        Subject = "Subject",
        AuthorId = Guid.NewGuid(),
        Pseudonym = "Spartak Ryabtsev",
        Content = "Content",
        Preview = "Preview"
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
    public void ShouldThrowValidationExceptionWhenAuthorNameIsTooLong()
    {
      var authorName = request.Pseudonym.PadLeft(100);

      validator.ShouldHaveValidationErrorFor(x => x.Pseudonym, authorName);
    }

    [Test]
    public void ShouldThrowValidationExceptionWhenContentIsEmpty()
    {
      validator.ShouldHaveValidationErrorFor(x => x.Subject, "");
    }

    [Test]
    public void ShouldNotHaveAnyValidationErrorsWhenRequestIsValid()
    {
      validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
    }
  }
}
*/
