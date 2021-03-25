using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interface;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.NewsService.Business
{
    /// <summary>
    /// Represents command class in command pattern. Provides method for getting user model by id.
    /// </summary>
    public class GetNewsByIdCommand : IGetNewsByIdCommand
    {
        private readonly INewsRepository _repository;
        private readonly INewsResponseMapper _mapper;

        /// <summary>
        /// Initialize new instance of <see cref="GetUserByIdCommand"/> class with specified repository.
        /// </summary>
        /// <param name="repository">Specified repository.</param>
        /// <param name="mapper">Specified mapper that convert user model from database to user model for response.</param>
        public GetNewsByIdCommand(INewsRepository repository, INewsResponseMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Return user model by specified user's id from UserServiceDb.
        /// </summary>
        /// <param name="newsId">Specified user's id.</param>
        /// <returns>User model with specified id.</returns>
        public News Execute(Guid newsId)
            => _mapper.Map(_repository.GetNewsInfoById(newsId));
    }
}
