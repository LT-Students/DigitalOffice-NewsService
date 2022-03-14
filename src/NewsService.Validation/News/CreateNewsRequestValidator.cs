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
    public CreateNewsRequestValidator()
    {
      RuleFor(news => news.TagsIds)
        .NotNull().WithMessage("Tags list must not be null.");

      RuleFor(news => news.Subject)
        .NotEmpty().WithMessage("Subject must not be empty.");

      RuleFor(news => news.Preview)
        .NotEmpty().WithMessage("Preview must not be empty.");

      RuleFor(news => news.Content)
        .NotEmpty().WithMessage("Content must not be empty.");
    }
  }
}
