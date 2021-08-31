using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.NewsService.Controllers
{
    [Route("[controller]")]
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
        public bool Edit(
            [FromServices] IEditNewsCommand command,
            [FromQuery] Guid newsId,
            [FromBody] JsonPatchDocument<EditNewsRequest> request)
        {
            return command.Execute(newsId, request);
        }

        [HttpPost("create")]
        public OperationResultResponse<Guid> Create(
            [FromServices] ICreateNewsCommand command,
            [FromBody] News request)
        {
            return command.Execute(request);
        }

        [HttpGet("find")]
        public List<NewsResponse> Find(
            [FromServices] IFindNewsCommand command,
            [FromQuery] FindNewsFilter findNewsFilter)
        {
            return command.Execute(findNewsFilter);
        }
    }
}