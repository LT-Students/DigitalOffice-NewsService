using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;

namespace LT.DigitalOffice.NewsService.Validation.Interfaces
{
  [AutoInject]
  public interface ICreateNewsRequestValidator : IValidator<CreateNewsRequest>
  {
  }
}
