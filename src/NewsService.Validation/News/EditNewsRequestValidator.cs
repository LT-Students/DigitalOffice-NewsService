using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.NewsService.Data.Interfaces;
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
    private readonly IChannelRepository _channelRepository;
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

      AddFailureForPropertyIf(
        nameof(EditNewsRequest.PublishedBy),
        x => x == OperationType.Replace,
        new()
        {
          { x => Guid.TryParse(x.value.ToString(), out Guid _), "Incorrect format of UserId." },
        });

      await AddFailureForPropertyIfAsync(
        nameof(EditNewsRequest.PublishedBy),
        x => x == OperationType.Replace,
        new()
        {
          {
            async x =>
            Guid.TryParse(x.value.ToString(), out Guid id)
            ? await CheckUserExistenceAsync(new List<Guid> { id }, new List<string>())
            : true,
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

      await AddFailureForPropertyIfAsync(
        nameof(EditNewsRequest.ChannelId),
        x => x == OperationType.Replace,
        new()
        {
          {
            async x =>
            Guid.TryParse(x.value.ToString(), out Guid id)
            ? await _channelRepository.DoesChannelExistAsync(Guid.Parse(x.value.ToString()))
            : true,
            "This channel doesn't exist."
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
      ILogger<CreateNewsRequestValidator> logger,
      IChannelRepository channelRepository)
    {
      _rcCheckUsersExistence = rcCheckUsersExistence;
      _logger = logger;
      _channelRepository = channelRepository;

      RuleForEach(x => x.Operations)
        .CustomAsync(async (x, context, token) => await HandleInternalPropertyValidation(x, context));
    }

    private async Task<bool> CheckUserExistenceAsync(List<Guid> users, List<string> errors)
    {
      if (!users.Any())
      {
        return false;
      }
      ICheckUsersExistence response = await RequestHandler.ProcessRequest<ICheckUsersExistence, ICheckUsersExistence>(
        _rcCheckUsersExistence,
        ICheckUsersExistence.CreateObj(users),
        errors,
        _logger);

      return users.Count == response.UserIds.Count;
    }
  }
}
