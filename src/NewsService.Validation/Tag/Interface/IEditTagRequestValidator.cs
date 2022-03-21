using System;
using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Tag;

namespace LT.DigitalOffice.NewsService.Validation.Tag.Interface
{
  [AutoInject]
  public interface IEditTagRequestValidator : IValidator<(Guid, EditTagsRequest)>
  {
  }
}
