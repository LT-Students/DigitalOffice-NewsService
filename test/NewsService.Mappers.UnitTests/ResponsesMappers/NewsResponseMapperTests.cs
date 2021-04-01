using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
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
        private Mock<IRequestClient<IGetUserDataRequest>> _requestClientMock;
        private Mock<ILogger<NewsResponseMapper>> _loggerMock;
        private Mock<Response<IOperationResult<IGetUserDataResponse>>> _brokerResponseAuthorNameMock;
        private Mock<Response<IOperationResult<IGetUserDataResponse>>> _brokerResponseSenderNameMock;
        private Mock<IOperationResult<IGetUserDataResponse>> _responseMock;

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
                AuthorName = "Pseudonym",
                AuthorId = user.Id,
                SenderId = user.Id,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        [SetUp]
        public void SetUp()
        {
            _responseMock = new Mock<IOperationResult<IGetUserDataResponse>>();
            _responseMock
                .Setup(x => x.Body.FirstName)
                .Returns(firstName);
            _responseMock = new Mock<IOperationResult<IGetUserDataResponse>>();
            _responseMock
                .Setup(x => x.Body.LastName)
                .Returns(lastName);
            _responseMock
                .Setup(x => x.IsSuccess)
                .Returns(true);
            _responseMock = new Mock<IOperationResult<IGetUserDataResponse>>();
            _responseMock
                .Setup(x => x.Body.MiddleName)
                .Returns(middleName);

            _brokerResponseAuthorNameMock = new Mock<Response<IOperationResult<IGetUserDataResponse>>>();
            _brokerResponseAuthorNameMock
                .Setup(x => x.Message)
                .Returns(_responseMock.Object);

            _brokerResponseSenderNameMock = new Mock<Response<IOperationResult<IGetUserDataResponse>>>();
            _brokerResponseSenderNameMock
                .Setup(x => x.Message)
                .Returns(_responseMock.Object);

            _requestClientMock = new Mock<IRequestClient<IGetUserDataRequest>>();
            _requestClientMock
                .Setup(x => x.GetResponse<IOperationResult<IGetUserDataResponse>>(
                    IGetUserDataRequest.CreateObj(dbNews.AuthorId), default, default))
                .Returns(Task.FromResult(_brokerResponseAuthorNameMock.Object));

                 _requestClientMock
                .Setup(x => x.GetResponse<IOperationResult<IGetUserDataResponse>>(
                    IGetUserDataRequest.CreateObj(dbNews.SenderId), default, default))
                .Returns(Task.FromResult(_brokerResponseSenderNameMock.Object));

            _loggerMock = new Mock<ILogger<NewsResponseMapper>>();
            _mapper = new NewsResponseMapper(_requestClientMock.Object, _loggerMock.Object);
        }

        [Test]
        public void ShouldThrowBadRequestExceptionWhenDbNewsIsNull()
        {
            Assert.Throws<BadRequestException>(() => _mapper.Map(null));
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
            expectedResult.Sender = new User { Id = dbNews.Id };
            expectedResult.Author = new User { Id = dbNews.Id };

            SerializerAssert.AreEqual(expectedResult, resultNewsModel);
        }

        [Test]
        public void ShouldThrowExeprionWhenLoggerHaveIt()
        {
             _requestClientMock
                .Setup(x => x.GetResponse<IOperationResult<IGetUserDataResponse>>(
                    IGetUserDataRequest.CreateObj(dbNews.SenderId), default, default))
                .Throws(new Exception());

            _mapper.Map(dbNews);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }

        [Test]
        public void ShouldExeprionWhen()
        {
           /* var responseMock = new Mock<IOperationResult<IGetUserDataResponse>>();
            responseMock
               .Setup(x => x.IsSuccess)
               .Returns(false);

            responseMock
               .Setup(x => x.Errors)
               .Returns(new List<string>() {"Not found senderId"});

            _brokerResponseSenderNameMock
               .Setup(x => x.Message)
               .Returns(responseMock.Object);

            _requestClientMock*/
             /*  .Setup(x => x.GetResponse<IOperationResult<IGetUserDataResponse>>(
                   IGetUserDataRequest.CreateObj(dbNews.SenderId), default, default))
               .Returns(Task.FromResult(_brokerResponseSenderNameMock.Object));
*//*
            _mapper.Map(dbNews);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);*/
        }

    }
}
