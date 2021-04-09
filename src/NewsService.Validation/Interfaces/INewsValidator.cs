using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.NewsService.Validation.Interfaces
{
    [AutoInject]
    public interface INewsValidator : IValidator<News>
    {
    }
}
