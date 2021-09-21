using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.NewsService.Validation
{
  public class EditNewsRequestValidator : BaseEditRequestValidator<EditNewsRequest>, IEditNewsRequestValidator
  {
    private readonly IRequestClient<ICheckUsersExistence> _rcCheckUsersExistence;
    private readonly IRequestClient<ICheckDepartmentsExistence> _rcCheckDepartmentsExistence;
    private readonly ILogger<CreateNewsRequestValidator> _logger;

    private void HandleInternalPropertyValidation(Operation<EditNewsRequest> requestedOperation, CustomContext context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

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
          { x => x.value.ToString().Length < 30, "Pseudonym is too long." },
        });

      #endregion

      #region AuthorId, DepartmentId

      AddFailureForPropertyIf(
          nameof(EditNewsRequest.DepartmentId),
          x => x == OperationType.Replace,
          new()
          {
            { x => Guid.TryParse(x.value.ToString(), out Guid result), "Department id has incorrect format" },
            { x => CheckDepartmentExistence(new List<Guid> { Guid.Parse(x.value.ToString()) }), "This department doesn't exist." }
          });

      AddFailureForPropertyIf(
        nameof(EditNewsRequest.AuthorId),
        x => x == OperationType.Replace,
        new()
        {
          { x => Guid.TryParse(x.value.ToString(), out Guid result), "Department id has incorrect format" },
          { x => CheckUserExistence(new List<Guid> { Guid.Parse(x.value.ToString()) }), "This user doesn't exist." }
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
    public EditNewsRequestValidator(
      IRequestClient<ICheckUsersExistence> rcCheckUsersExistence,
      IRequestClient<ICheckDepartmentsExistence> rcCheckDepartmentsExistence,
      ILogger<CreateNewsRequestValidator> logger)
    {
      _rcCheckUsersExistence = rcCheckUsersExistence;
      _rcCheckDepartmentsExistence = rcCheckDepartmentsExistence;
      _logger = logger;

      RuleForEach(x => x.Operations)
        .Custom(HandleInternalPropertyValidation);
    }

    private bool CheckUserExistence(List<Guid> authorsIds)
    {
      if (!authorsIds.Any())
      {
        return false;
      }

      try
      {
        Response<IOperationResult<ICheckUsersExistence>> response =
          _rcCheckUsersExistence.GetResponse<IOperationResult<ICheckUsersExistence>>(
          ICheckUsersExistence.CreateObj(authorsIds)).Result;

        if (response.Message.IsSuccess)
        {
          return authorsIds.Count == response.Message.Body.UserIds.Count;
        }

        _logger.LogWarning("Can not find author Ids: {authorsIds}: " +
          $"{Environment.NewLine}{string.Join('\n', response.Message.Errors)}");
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, "Cannot check existing authors withs this ids {authorsIds}");
      }

      return false;
    }

    private bool CheckDepartmentExistence(List<Guid> departmentsIds)
    {
      if (!departmentsIds.Any())
      {
        return false;
      }

      try
      {
        Response<IOperationResult<ICheckDepartmentsExistence>> response =
          _rcCheckDepartmentsExistence.GetResponse<IOperationResult<ICheckDepartmentsExistence>>(
          ICheckDepartmentsExistence.CreateObj(departmentsIds)).Result;

        if (response.Message.IsSuccess)
        {
          return departmentsIds.Count == response.Message.Body.DepartmentIds.Count;
        }

        _logger.LogWarning($"Can not find Department Ids: {departmentsIds}: " +
          $"{Environment.NewLine}{string.Join('\n', response.Message.Errors)}");
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, $"Cannot check existing Departments withs this id {departmentsIds}");
      }

      return false;
    }
  }
}
