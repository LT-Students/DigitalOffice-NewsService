using FluentValidation;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Validation
{
    public class PatchNewsValidator : AbstractValidator<JsonPatchDocument<EditNewsRequest>>, IPatchNewsValidator
    {
        public PatchNewsValidator()
        {

        }
    }
}
