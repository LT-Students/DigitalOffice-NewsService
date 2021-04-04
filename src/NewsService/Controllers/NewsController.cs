using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.ModelResponse;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        [HttpGet("getNewsById")]
        public NewsResponse GetNewsInfoById(
            [FromServices] IGetNewsByIdCommand command,
            [FromQuery] Guid newsId)
        {
            return command.Execute(newsId);
        }

        [HttpPost("editNews")]
        public void EditNews(
            [FromServices] IEditNewsCommand command,
            [FromBody] News request)
        {
            command.Execute(request);
        }

        [HttpPost("createNews")]
        public Guid CreateNews(
            [FromServices] ICreateNewsCommand command,
            [FromBody] News request)
        {
            return command.Execute(request);
        }

        [HttpGet("findnews")]
        public List<NewsResponse> FindNews(
            [FromServices] IFindNewsCommand command,
            [FromQuery] Guid? authorId,
            [FromQuery] Guid? departmentId,
            [FromQuery] string Pseudonym,
            [FromQuery] string subject)
        {
            FindNewsParams findNewsParams = new FindNewsParams
            {
                AuthorId = authorId,
                DepartmentId = departmentId,
                Pseudonym = Pseudonym,
                Subject = subject
            };

            return command.Execute(findNewsParams);
        }
    }
}