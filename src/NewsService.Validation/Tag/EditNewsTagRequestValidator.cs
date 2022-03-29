using System;
using System.Linq;
using FluentValidation;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Tag;
using LT.DigitalOffice.NewsService.Validation.Tag.Interface;

namespace LT.DigitalOffice.NewsService.Validation.Tag
{
  public class EditNewsTagRequestValidator : AbstractValidator<(Guid id, EditNewsTagsRequest request)>, IEditTagRequestValidator
  {
    public EditNewsTagRequestValidator(
      INewsTagRepository repository)
    {
      RuleFor(tag => tag.request.TagsToAdd)
        .Cascade(CascadeMode.Stop)
        .NotNull().WithMessage("TagsToAdd list must not be null.")
        .Must(t => t.Distinct().ToList().Count() == t.Count())
        .WithMessage("The tags can't be duplicated.")
        .MustAsync(async (request, listTag, _) => !await repository.DoNewsTagsIdsExist(request.id, listTag))
        .WithMessage("The tags is already added.");

      RuleFor(tag => tag.request.TagsToRemove)
        .Cascade(CascadeMode.Stop)
        .NotNull().WithMessage("TagsToRemove list must not be null.")
        .Must(x => x.Distinct().ToList().Count() == x.Count())
        .WithMessage("The tags can't be duplicated.");
    }
  }
}
