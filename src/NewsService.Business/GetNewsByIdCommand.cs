using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interface;
using LT.DigitalOffice.NewsService.Models.Dto.ModelResponse;
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

        public NewsResponse Execute(Guid newsId)
        {
            return _mapper.Map(_repository.GetNewsInfoById(newsId));
        }
    }
}
