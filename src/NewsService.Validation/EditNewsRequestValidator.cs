using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.NewsService.Validation
{
  public class EditNewsRequestValidator : BaseEditRequestValidator<EditNewsRequest>, IEditNewsRequestValidator
  {
    private void HandleInternalPropertyValidation(Operation<EditNewsRequest> requestedOperation, CustomContext context)
    {
      #region local functions

      void AddСorrectPaths(List<string> paths)
      {
        if (paths.FirstOrDefault(p => p.EndsWith(requestedOperation.path[1..], StringComparison.OrdinalIgnoreCase)) == null)
        {
          context.AddFailure(requestedOperation.path, $"This path {requestedOperation.path} is not available");
        }
      }

      void AddСorrectOperations(
          string propertyName,
          List<OperationType> types)
      {
        if (requestedOperation.path.EndsWith(propertyName, StringComparison.OrdinalIgnoreCase)
            && !types.Contains(requestedOperation.OperationType))
        {
          context.AddFailure(propertyName, $"This operation {requestedOperation.OperationType} is prohibited for {propertyName}");
        }
      }

      void AddFailureForPropertyIf(
          string propertyName,
          Func<OperationType, bool> type,
          Dictionary<Func<Operation<EditNewsRequest>, bool>, string> predicates)
      {
        if (!requestedOperation.path.EndsWith(propertyName, StringComparison.OrdinalIgnoreCase)
            || !type(requestedOperation.OperationType))
        {
          return;
        }

        foreach (var validateDelegate in predicates)
        {
          if (!validateDelegate.Key(requestedOperation))
          {
            context.AddFailure(propertyName, validateDelegate.Value);
          }
        }
      }

      #endregion

      #region Paths

      AddСorrectPaths(
        new()
        {
          nameof(EditNewsRequest.Preview),
          nameof(EditNewsRequest.Content),
          nameof(EditNewsRequest.Subject),
          nameof(EditNewsRequest.Pseudonym),
          nameof(EditNewsRequest.AuthorId),
          nameof(EditNewsRequest.DepartmentId),
          nameof(EditNewsRequest.IsActive),
        });

      AddСorrectOperations(nameof(EditNewsRequest.Preview), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditNewsRequest.Content), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditNewsRequest.Subject), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditNewsRequest.Pseudonym), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditNewsRequest.AuthorId), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditNewsRequest.DepartmentId), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditNewsRequest.IsActive), new() { OperationType.Replace });

      #endregion

      #region string property

      AddFailureForPropertyIf(
        nameof(EditNewsRequest.Preview),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value.ToString()), "Preview cannot be empty." },
        });

      AddFailureForPropertyIf(
        nameof(EditNewsRequest.Content),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value.ToString()), "Content cannot be empty." },
        });

      AddFailureForPropertyIf(
        nameof(EditNewsRequest.Subject),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value.ToString()), "Subject cannot be empty." },
          { x => x.value.ToString().Length < 120, "Subject is too long." },
        });

      AddFailureForPropertyIf(
        nameof(EditNewsRequest.Pseudonym),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value.ToString()), "Pseudonym cannot be empty." },
          { x => x.value.ToString().Length < 30, "Pseudonym is too long." },
        });

      #endregion

      #region AuthorId, DepartmentId

      AddFailureForPropertyIf(
        nameof(EditNewsRequest.DepartmentId),
        x => x == OperationType.Replace,
        new()
        {
          { x => Guid.TryParse(x.value.ToString(), out Guid result), "Department id has incorrect format" }
        });

      AddFailureForPropertyIf(
        nameof(EditNewsRequest.AuthorId),
        x => x == OperationType.Replace,
        new()
        {
          { x => Guid.TryParse(x.value.ToString(), out Guid result), "Author id has incorrect format" }
        });

      #endregion

      #region IsActive

      AddFailureForPropertyIf(
        nameof(EditNewsRequest.IsActive),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x.value?.ToString(), out _), "Incorrect IsActive format." },
        });

      #endregion
    }
    public EditNewsRequestValidator()
    {
      RuleForEach(x => x.Operations)
        .Custom(HandleInternalPropertyValidation);
    }
  }
}
