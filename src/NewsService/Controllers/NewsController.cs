using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.NewsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        [HttpGet("getNewsById")]
        public News GetNewsById(
            [FromServices] IGetNewsByIdCommand getNewsByIdCommand, 
            [FromQuery] Guid newsId)
        {
            return getNewsByIdCommand.Execute(newsId);
        }

        [HttpPost("editNews")]
        public void EditNews(
            [FromServices] IEditNewsCommand editNewsCommand,
            [FromBody] News request)
        {
            editNewsCommand.Execute(request);
        }

        [HttpPost("createNews")]
        public Guid CreateNews(
            [FromServices] ICreateNewsCommand createNewsCommand,
            [FromBody] News request)
        {
            return createNewsCommand.Execute(request);
        }
    }
}