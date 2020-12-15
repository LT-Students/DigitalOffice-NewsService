using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.NewsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        [HttpPost("createNews")]
        public Guid CreateNews(
            [FromServices] ICreateNewsCommand command,
            [FromBody] News request)
        {
            return command.Execute(request);
        }

        [HttpPatch("editNews")]
        public void CreateNews(
            [FromServices] IEditNewsCommand command,
            [FromHeader] Guid userId,
            [FromQuery] Guid newId,
            [FromBody] JsonPatchDocument<DbNews> patch)
        {
            command.Execute(userId, newId, patch);
        }
    }
}