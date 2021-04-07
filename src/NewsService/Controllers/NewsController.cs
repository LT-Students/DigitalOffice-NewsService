using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPatch("edit")]
        public void Edit(
            [FromServices] IEditNewsCommand command,
            [FromQuery] Guid newsId,
            [FromBody] JsonPatchDocument<EditNewsRequest> request)
        {
            command.Execute(request);
        }

        [HttpPost("create")]
        public Guid Create(
            [FromServices] ICreateNewsCommand command,
            [FromBody] News request)
        {
            return command.Execute(request);
        }

        [HttpGet("find")]
        public List<NewsResponse> Find(
            [FromServices] IFindNewsCommand command,
            [FromQuery] Guid? authorId,
            [FromQuery] Guid? departmentId,
            [FromQuery] string pseudonym,
            [FromQuery] string subject)
        {
            FindNewsParams findNewsParams = new FindNewsParams
            {
                AuthorId = authorId,
                DepartmentId = departmentId,
                Pseudonym = pseudonym,
                Subject = subject
            };

            return command.Execute(findNewsParams);
        }
    }
}