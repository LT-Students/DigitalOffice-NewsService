using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Validation.Interfaces
{
    [AutoInject]
    public interface IPatchNewsValidator : IValidator<JsonPatchDocument<EditNewsRequest>>
    {
    }
}
