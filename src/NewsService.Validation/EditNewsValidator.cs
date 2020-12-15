using FluentValidation;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.NewsService.Validation
{
    public class EditNewsValidator : AbstractValidator<JsonPatchDocument<DbNews>>
    {
        private static string ContentPath => $"/{nameof(DbNews.Content)}";
        private static string SubjectPath => $"/{nameof(DbNews.Subject)}";
        private static string AuthorNamePatch => $"/{nameof(DbNews.AuthorName)}";

        private static List<string> Paths
            => new List<string> { ContentPath, SubjectPath, AuthorNamePatch };

        Func<JsonPatchDocument<DbNews>, string, Operation> GetOperationByPath =>
            (x, path) => x.Operations.FirstOrDefault(x => x.path == path);

        public EditNewsValidator()
        {
            RuleFor(x => x.Operations)
                .Must(x => x.Select(x => x.path).Distinct().Count() == x.Count())
                .WithMessage("You don't have to change the same field of Project multiple times.")
                .Must(x => x.Any())
                .WithMessage("You don't have changes.")
                .ForEach(x => x
                .Must(x => Paths.Contains(x.path))
                .WithMessage($"Document contains invalid path. Only such paths are allowed: {Paths.Aggregate((x, y) => x + ", " + y)}")
                )
                .DependentRules(() =>
                {
                    When(x => GetOperationByPath(x, ContentPath) != null, () =>
                    {
                        RuleFor(x => x.Operations)
                            .UniqueOperationWithAllowedOp(ContentPath, "add", "replace");

                        RuleFor(x => (string)GetOperationByPath(x, ContentPath).value)
                            .NotEmpty()
                            .WithMessage($"{ContentPath} must not be empty.");
                    });

                    When(x => GetOperationByPath(x, SubjectPath) != null, () =>
                    {
                        RuleFor(x => x.Operations)
                            .UniqueOperationWithAllowedOp(SubjectPath, "add", "replace");

                        RuleFor(x => (string)GetOperationByPath(x, SubjectPath).value)
                            .NotEmpty()
                            .WithMessage($"{SubjectPath} must not be empty.")
                            .MaximumLength(120)
                            .WithMessage($"News {SubjectPath} is too long.");
                    });

                    When(x => GetOperationByPath(x, AuthorNamePatch) != null, () =>
                    {
                        RuleFor(x => x.Operations)
                            .UniqueOperationWithAllowedOp(AuthorNamePatch, "add", "replace");

                        RuleFor(x => (string)GetOperationByPath(x, AuthorNamePatch).value)
                            .NotEmpty()
                            .WithMessage($"{AuthorNamePatch} must not be empty.")
                            .MaximumLength(50)
                            .WithMessage($"News {AuthorNamePatch} is too long.");
                    });
                });
        }
    }
}
