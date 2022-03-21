using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Tag;
using LT.DigitalOffice.NewsService.Validation.Tag.Interface;

namespace LT.DigitalOffice.NewsService.Validation.Tag
{
  public class EditTagRequestValidator : AbstractValidator<(Guid, EditTagsRequest)>, IEditTagRequestValidator
  {
    public EditTagRequestValidator(
      INewsTagRepository repository)
    {
      RuleFor(tag => tag.Item2.TagsToAdd)
        .Cascade(CascadeMode.Stop)
        .NotNull().WithMessage("TagsToAdd list must not be null.")
        .Must((id, tag) => id.Item2.TagsToAdd.Distinct().ToList().Count() == id.Item2.TagsToAdd.Count())
        .WithMessage("The tags can't be duplicated.")
        .MustAsync(async (id, listTag, _) => !await repository.DoNewsTagsIdsExist(id.Item1, listTag))
        .WithMessage("The tags is already added.");


      RuleFor(tag => tag.Item2.TagsToRemove)
        .Cascade(CascadeMode.Stop)
        .NotNull().WithMessage("TagsToRemove list must not be null.")
        .Must(x => x.Distinct().ToList().Count() == x.Count())
        .WithMessage("The tags can't be duplicated.");
    }
  }
}
