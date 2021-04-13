using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.NewsService.Validation.UnitTests
{
    public class EditNewsValidatorTests
    {
        private IValidator<JsonPatchDocument<EditNewsRequest>> _validator;
        private JsonPatchDocument<EditNewsRequest> _editNewsRequest;

        Func<string, Operation> GetOperationByPath =>
            (path) => _editNewsRequest.Operations.Find(x => x.path == path);


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new EditNewsValidator();
        }

        [SetUp]
        public void SetUp()
        {
            _editNewsRequest = new JsonPatchDocument<EditNewsRequest>(new List<Operation<EditNewsRequest>>
            {
                new Operation<EditNewsRequest>(
                    "replace",
                    $"/{nameof(EditNewsRequest.Subject)}",
                    "",
                    "Subject"),
                new Operation<EditNewsRequest>(
                    "replace",
                    $"/{nameof(EditNewsRequest.Content)}",
                    "",
                    "Content"),
                new Operation<EditNewsRequest>(
                    "replace",
                    $"/{nameof(EditNewsRequest.IsActive)}",
                    "",
                    false),
            }, new CamelCasePropertyNamesContractResolver());
        }

        [Test]
        public void SuccessValidation()
        {
            _validator.TestValidate(_editNewsRequest).ShouldNotHaveAnyValidationErrors();
        }

        #region Base validate errors

        [Test]
        public void ExceptionNotContainsOperations()
        {
            _editNewsRequest.Operations.Clear();

            _validator.TestValidate(_editNewsRequest).ShouldHaveAnyValidationError();
        }

        [Test]
        public void ExceptionNotUniqueOperations()
        {
            _editNewsRequest.Operations.Add(_editNewsRequest.Operations.First());

            _validator.TestValidate(_editNewsRequest).ShouldHaveAnyValidationError();
        }

        [Test]
        public void ExceptionNotSupportedReplace()
        {
            _editNewsRequest.Operations.Add(new Operation<EditNewsRequest>("replace", $"/{nameof(DbNews.Id)}", "", Guid.NewGuid()));

            _validator.TestValidate(_editNewsRequest).ShouldHaveAnyValidationError();
        }
        #endregion

        #region Names size checks

        [Test]
        public void ExceptionSubjectIsTooLong()
        {
            GetOperationByPath(EditNewsValidator.Subject).value = "".PadLeft(33);

            _validator.TestValidate(_editNewsRequest).ShouldHaveAnyValidationError();
        }

        [Test]
        public void ExceptionSubjectIsTooShort()
        {
            GetOperationByPath(EditNewsValidator.Subject).value = "";

            _validator.TestValidate(_editNewsRequest).ShouldHaveAnyValidationError();
        }

        [Test]
        public void ExceptionEmptyContent()
        {
            GetOperationByPath(EditNewsValidator.Content).value = "";

            _validator.TestValidate(_editNewsRequest).ShouldHaveAnyValidationError();
        }

        /*[Test]
        public void ExceptionEmptyIsActive()
        {
            GetOperationByPath(EditNewsValidator.IsActive).value = null;

            _validator.TestValidate(_editNewsRequest).ShouldHaveAnyValidationError();
        }*/
        #endregion
    }
}
