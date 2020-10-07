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
        [HttpPost("editNews")]
        public void CreateNews(
            [FromServices] IEditNewsCommand command,
            [FromBody] NewsRequest request)
        {
            command.Execute(request);
        }

        [HttpPost("createNews")]
        public Guid CreateNews(
            [FromServices] ICreateNewsCommand command,
            [FromBody] CreateNewsRequest request)
        {
            return command.Execute(request);
        }
    }
}