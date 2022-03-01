using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.NewsService.Validation
{
  public class EditNewsRequestValidator : BaseEditRequestValidator<EditNewsRequest>, IEditNewsRequestValidator
  {
    private readonly IRequestClient<ICheckUsersExistence> _rcCheckUsersExistence;
    private readonly ILogger<CreateNewsRequestValidator> _logger;

    private async Task HandleInternalPropertyValidation(
      Operation<EditNewsRequest> requestedOperation,
      CustomContext context)
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
          nameof(EditNewsRequest.ChannelId),
          nameof(EditNewsRequest.PublishedBy),
          nameof(EditNewsRequest.IsActive),
        });

      AddСorrectOperations(nameof(EditNewsRequest.Preview), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditNewsRequest.Content), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditNewsRequest.Subject), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditNewsRequest.ChannelId), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditNewsRequest.PublishedBy), new() { OperationType.Replace });
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

      #endregion

      #region PublishedBy

      await AddFailureForPropertyIfAsync(
        nameof(EditNewsRequest.PublishedBy),
        x => x == OperationType.Replace,
        new()
        {
          {
            async x =>
            Guid.TryParse(x.value.ToString(), out Guid id) &&
            await CheckUserExistenceAsync(new List<Guid> { id }),
            "This user doesn't exist."
          }
        });

      #endregion

      #region ChanelId

      AddFailureForPropertyIf(
        nameof(EditNewsRequest.ChannelId),
        x => x == OperationType.Replace,
        new()
        {
          {
            x =>
            Guid.TryParse(x.value.ToString(), out Guid id),
            "Incorrect chanelId format."
          }
        });

      #endregion

      #region IsActive

      AddFailureForPropertyIf(
        nameof(EditNewsRequest.IsActive),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x.value?.ToString(), out _), "Incorrect is active format." },
        });

      #endregion
    }
    public EditNewsRequestValidator(
      IRequestClient<ICheckUsersExistence> rcCheckUsersExistence,
      ILogger<CreateNewsRequestValidator> logger)
    {
      _rcCheckUsersExistence = rcCheckUsersExistence;
      _logger = logger;

      RuleForEach(x => x.Operations)
        .CustomAsync(async (x, context, token) => await HandleInternalPropertyValidation(x, context));
    }

    private async Task<bool> CheckUserExistenceAsync(List<Guid> usersIds)
    {
      if (!usersIds.Any() || usersIds == default)
      {
        return false;
      }

      try
      {
        Response<IOperationResult<ICheckUsersExistence>> response =
          await _rcCheckUsersExistence.GetResponse<IOperationResult<ICheckUsersExistence>>(
            ICheckUsersExistence.CreateObj(usersIds));

        if (response.Message.IsSuccess)
        {
          return usersIds.Count == response.Message.Body.UserIds.Count;
        }

        _logger.LogWarning(
          "Errors while check users existence Ids: {UsersIds}. \n Errors: {Errors}",
          string.Join(", ", usersIds),
          string.Join('\n', response.Message.Errors));
      }
      catch (Exception exc)
      {
        _logger.LogError(
          exc,
          "Cannot check departments existence Ids: {UsersIds}",
          string.Join(", ", usersIds));
      }

      return false;
    }
  }
}
