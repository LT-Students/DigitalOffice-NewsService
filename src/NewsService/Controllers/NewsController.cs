using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto;
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
            [FromBody] CreateNewsRequest request)
        {
            return command.Execute(request);
        }

        [HttpGet("getNews")]
        public News GetNews(
            [FromServices] IGetNewsCommand command,
            [FromBody] Guid newsId)
        {
            return command.Execute(newsId);
        }
    }
}
