using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.NewsService.Validation
{
  public class CreateNewsRequestValidator : AbstractValidator<CreateNewsRequest>, ICreateNewsRequestValidator
  {
    private readonly IRequestClient<ICheckUsersExistence> _rcCheckUsersExistence;
    private readonly ILogger<CreateNewsRequestValidator> _logger;

    public CreateNewsRequestValidator(
      IRequestClient<ICheckUsersExistence> rcCheckUsersExistence,
      ILogger<CreateNewsRequestValidator> logger)
    {
      _rcCheckUsersExistence = rcCheckUsersExistence;
      _logger = logger;

      When(request => request.PublishedBy != null, () =>
      {
        RuleFor(news => news.PublishedBy)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("Publisher Id can not be empty.")
        .MustAsync(async (publisher, cancellation) => await CheckUserExistenceAsync(new List<Guid>() { publisher.Value }, new List<string>()))
        .WithMessage("This publisher doesn't exist.");
      });

      RuleFor(news => news.Subject)
        .NotEmpty().WithMessage("Subject must not be empty.");

      RuleFor(news => news.Preview)
        .NotEmpty().WithMessage("Preview must not be empty.");

      RuleFor(news => news.Content)
        .NotEmpty().WithMessage("Content must not be empty.");
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
