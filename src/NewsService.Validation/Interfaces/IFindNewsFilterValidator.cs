using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;

namespace LT.DigitalOffice.NewsService.Validation.Interfaces
{
  [AutoInject]
  public interface IFindNewsFilterValidator : IValidator<FindNewsFilter>
  {
  }
}
