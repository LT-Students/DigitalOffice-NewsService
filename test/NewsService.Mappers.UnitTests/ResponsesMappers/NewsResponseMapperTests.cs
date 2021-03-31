using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.NewsService.Business;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interface;
using LT.DigitalOffice.NewsService.Models.Broker.Requests;
using LT.DigitalOffice.NewsService.Models.Broker.Responses;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Model;
using LT.DigitalOffice.NewsService.Models.Dto.ModelResponse;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.NewsService.Mappers.UnitTests.ResponsesMappers
{
    internal class NewsResponseMapperTests
    {
        //private Mock<INewsResponseMapper> mapperMock;
        private Mock<INewsRepository> repositoryMock;

        private Mock<IRequestClient<IGetUserDataRequest>> _requestClientMock;//1
        private Mock<ILogger<NewsResponseMapper>> _loggerMock;//2
        private Mock<Response<IOperationResult<IGetUserDataResponse>>> _brokerResponseMock;//4
        private Mock<IOperationResult<IGetUserDataResponse>> _responseMock;

        private Mock<IGetUserDataResponse> userResponseMock;
        private NewsResponseMapper newsResponseMapper;
        private const string firstName = "Ivan";
        private const string lastName = "Ivanov";
        private const string middleName = "Ivanovich";

        private INewsResponseMapper _mapper;
        private NewsResponse response;
        private DbNews dbNews;
        private User user;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                FIO = "Ivan Ivanov"
            };

            response = new NewsResponse
            {
                Id = Guid.NewGuid(),
                Content = "Content111",
                Subject = "Subject111",
                Author = user,
                Sender = user,
                CreatedAt = DateTime.UtcNow
            };

            dbNews = new DbNews
            {
                Id = response.Id,
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "Pseudonym",
                AuthorId = user.Id,
                SenderId = user.Id,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        [SetUp]
        public void SetUp()
        {
            //mapperMock = new Mock<INewsResponseMapper>();
            repositoryMock = new Mock<INewsRepository>();

            _brokerResponseMock = new Mock<Response<IOperationResult<IGetUserDataResponse>>>();//5
            _brokerResponseMock
                .Setup(x => x.Message)
                .Returns(_responseMock.Object);

            _responseMock = new Mock<IOperationResult<IGetUserDataResponse>>();//6
            /*_responseMock
               *//* .Setup(x => x.)
                .Returns(true);*/
            _responseMock
                .Setup(x => x.Body)
                .Returns(firstName);

            _loggerMock = new Mock<ILogger<NewsResponseMapper>>();//3
            _mapper = new NewsResponseMapper(_requestClientMock.Object, _loggerMock.Object);//3

            _requestClientMock = new Mock<IRequestClient<IGetUserDataRequest>>();//3
            _requestClientMock
                .Setup(x => x.GetResponse<IOperationResult<IGetUserDataResponse>>(
                    IGetUserDataRequest.CreateObj(It.IsAny<Guid>()), default, default))
                .Returns(Task.FromResult(_brokerResponseMock.Object));//4


            userResponseMock = new Mock<IGetUserDataResponse>();
            userResponseMock.Setup(x => x.Id).Returns(user.Id);// setup User
            userResponseMock.Setup(x => x.FirstName).Returns(user.FIO);

        }

        [Test]
        public void ShouldThrowBadRequestExceptionWhenDbNewsIsNull()
        {
            Assert.Throws<BadRequestException>(() => newsResponseMapper.Map(null));
        }

        [Test]
        public void ShouldReturnNewsResponseModelWhenMappingValidDbNews()
        {
            var resultNewsModel = _mapper.Map(dbNews);
            var expectedResult = new NewsResponse();

            expectedResult.Id = dbNews.Id;
            expectedResult.Subject = dbNews.Subject;
            expectedResult.CreatedAt = dbNews.CreatedAt;
            expectedResult.Content = dbNews.Content;
            expectedResult.Sender = new User()
            {
                Id = user.Id,
                FIO = user.FIO
            };
            expectedResult.Author = new User()
            {
                Id = user.Id,
                FIO = user.FIO
            };

            //repositoryMock.Setup(repository => repository.GetNewsInfoById(response.Id)).Returns(dbNews).Verifiable();
            SerializerAssert.AreEqual(expectedResult, resultNewsModel);
        }
    }
}
