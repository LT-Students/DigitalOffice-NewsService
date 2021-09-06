using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using System;

namespace LT.DigitalOffice.NewsService.Business
{
    public class GetNewsByIdCommand : IGetNewsByIdCommand
    {
        private readonly INewsRepository _repository;
        private readonly INewsResponseMapper _mapper;

        public GetNewsByIdCommand(INewsRepository repository, INewsResponseMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public OperationResultResponse<NewsResponse> Execute(Guid newsId)
        {
            OperationResultResponse<NewsResponse> response = new();

            response.Body = _mapper.Map(_repository.GetNewsInfoById(newsId));
            response.Status = OperationResultStatusType.FullSuccess;
            return response;
        }
    }
}
