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
        .MustAsync(async (publisher, cancellation) => await CheckUserExistence(new List<Guid>() { publisher.Value }, new List<string>()))
        .WithMessage("This publisher doesn't exist.");
      });

      RuleFor(news => news.Subject)
        .NotEmpty().WithMessage("Subject must not be empty.");

      RuleFor(news => news.Preview)
        .NotEmpty().WithMessage("Preview must not be empty.");

      RuleFor(news => news.Content)
        .NotEmpty().WithMessage("Content must not be empty.");
    }

    private async Task<bool> CheckUserExistence(List<Guid> publisherIds, List<string> errors)
    {
      if (!publisherIds.Any())
      {
        return false;
      }
      ICheckUsersExistence response = await RequestHandler.ProcessRequest<ICheckUsersExistence, ICheckUsersExistence>(
            _rcCheckUsersExistence,
            ICheckUsersExistence.CreateObj(publisherIds),
            errors,
            _logger);

        return publisherIds.Count == response.UserIds.Count;

      /* try
       {
         Response<IOperationResult<ICheckUsersExistence>> response =
           await _rcCheckUsersExistence.GetResponse<IOperationResult<ICheckUsersExistence>>(
             ICheckUsersExistence.CreateObj(publisherIds));

         if (response.Message.IsSuccess)
         {
           return publisherIds.Count == response.Message.Body.UserIds.Count;
         }

         _logger.LogWarning("Can not find author Ids: {authorsIds}: " +
           $"{Environment.NewLine}{string.Join('\n', response.Message.Errors)}");
       }
       catch (Exception exc)
       {
         _logger.LogError(exc, "Cannot check existing authors withs this ids {authorsIds}");
       }

       return false;*/
    }
  }
}
