using FluentValidation;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.NewsService.Validation
{
    public class EditNewsValidator : AbstractValidator<JsonPatchDocument<EditNewsRequest>>, IEditNewsValidator
    {
        private static List<string> Paths
            => new List<string> { Content, Subject, IsActive };

        public static string Content => $"/{nameof(EditNewsRequest.Content)}";
        public static string Subject => $"/{nameof(EditNewsRequest.Subject)}";
        public static string IsActive => $"/{nameof(EditNewsRequest.IsActive)}";

        Func<JsonPatchDocument<EditNewsRequest>, string, Operation> GetOperationByPath =>
            (x, path) =>
                x.Operations.FirstOrDefault(x =>
                    string.Equals(
                        x.path,
                        path,
                        StringComparison.OrdinalIgnoreCase));

        public EditNewsValidator()
        {
            RuleFor(x => x.Operations)
                .Must(x => x.Any())
                .WithMessage("You don't have changes.")
                .Must(x => x.Select(x => x.path).Distinct().Count() == x.Count())
                .WithMessage("You don't have to change the same field of News multiple times.")
                .ForEach(x => x
                    .Must(x => Paths.Contains(x.path))
                    .WithMessage($"Document contains invalid path. Only such paths are allowed: {Paths.Aggregate((x, y) => x + ", " + y)}")
                )
                .DependentRules(() =>
                {
                    When(x => GetOperationByPath(x, Content) != null, () =>
                    {
                        RuleFor(x => x.Operations)
                            .UniqueOperationWithAllowedOp(Content, "replace");

                        RuleFor(x => (string)GetOperationByPath(x, Content).value)
                            .NotEmpty()
                            .MinimumLength(1).WithMessage("Content is too short.");
                    });

                    When(x => GetOperationByPath(x, Subject) != null, () =>
                    {
                        RuleFor(x => x.Operations)
                            .UniqueOperationWithAllowedOp(Subject, "replace");

                        RuleFor(x => (string)GetOperationByPath(x, Subject).value)
                            .NotEmpty()
                            .MaximumLength(120)
                            .WithMessage("News subject is too long.");
                    });

                    When(x => GetOperationByPath(x, IsActive) != null, () =>
                    {
                        RuleFor(x => x.Operations)
                            .UniqueOperationWithAllowedOp(IsActive, "replace");

                        RuleFor(x => (string)GetOperationByPath(x, Content).value)
                            .NotEmpty();
                    });
                });
        }
    }
}
