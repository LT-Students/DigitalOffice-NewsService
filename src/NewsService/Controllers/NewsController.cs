using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
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

        [HttpGet("findNews")]
        public List<DbNews> FindNews(
            [FromServices] IFindNewsCommand command,
            [FromQuery] Guid? authorId,
            [FromQuery] Guid? departmentId,
            [FromQuery] string authorName,
            [FromQuery] string newsName)
        {
            FindNewsParams findNewsParams = new FindNewsParams();
            findNewsParams.AuthorId = authorId;
            findNewsParams.DepartmentId = departmentId;
            findNewsParams.AuthorId = authorId;
            findNewsParams.NewsName = newsName;

            return command.Execute(findNewsParams);
        }
    }
}