using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.NewsService.Validation
{
  public class CreateNewsRequestValidator : AbstractValidator<CreateNewsRequest>, ICreateNewsRequestValidator
  {
    private readonly IRequestClient<ICheckUsersExistence> _rcCheckUsersExistence;
    private readonly IRequestClient<ICheckDepartmentsExistence> _rcCheckDepartmentsExistence;
    private readonly ILogger<CreateNewsRequestValidator> _logger;

    public CreateNewsRequestValidator(
      IRequestClient<ICheckUsersExistence> rcCheckUsersExistence,
      IRequestClient<ICheckDepartmentsExistence> rcCheckDepartmentsExistence,
      ILogger<CreateNewsRequestValidator> logger)
    {
      _rcCheckUsersExistence = rcCheckUsersExistence;
      _rcCheckDepartmentsExistence = rcCheckDepartmentsExistence;
      _logger = logger;

      RuleFor(news => news.AuthorId)
        .NotEmpty().WithMessage("AuthorId must not be empty.")
        .MustAsync(async (authorId, cancellation) => await CheckUserExistence(new List<Guid>() { authorId }))
        .WithMessage("This author doesn't exist.");

      RuleFor(news => news.Pseudonym)
        .MaximumLength(50).WithMessage("Pseudonym is too long.");

      RuleFor(news => news.Subject)
        .NotEmpty().WithMessage("Subject must not be empty.")
        .MaximumLength(120).WithMessage("News subject is too long.");

      RuleFor(preview => preview.Preview)
        .NotEmpty().WithMessage("Preview must not be empty.");

      RuleFor(news => news.Content)
        .NotEmpty().WithMessage("Content must not be empty.");

      When(
        news => news.DepartmentId.HasValue,
        () =>
          RuleFor(news => news.DepartmentId)
            .MustAsync(async (departmentId, cancellation) => await CheckDepartmentsExistence(new List<Guid>() { departmentId.Value }))
            .WithMessage("This department doesn't exist."));
    }

    private async Task<bool> CheckUserExistence(List<Guid> authorsIds)
    {
      if (!authorsIds.Any())
      {
        return false;
      }

      try
      {
        Response<IOperationResult<ICheckUsersExistence>> response =
          await _rcCheckUsersExistence.GetResponse<IOperationResult<ICheckUsersExistence>>(
          ICheckUsersExistence.CreateObj(authorsIds));

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

    private async Task<bool> CheckDepartmentsExistence(List<Guid> departmentsIds)
    {
      if (!departmentsIds.Any())
      {
        return false;
      }

      try
      {
        Response<IOperationResult<ICheckDepartmentsExistence>> response =
          await _rcCheckDepartmentsExistence.GetResponse<IOperationResult<ICheckDepartmentsExistence>>(
          ICheckDepartmentsExistence.CreateObj(departmentsIds));

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
