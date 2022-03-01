using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Validation.Interfaces
{
  [AutoInject]
  public interface IEditNewsRequestValidator : IValidator<JsonPatchDocument<EditNewsRequest>>
  {
  }
}
