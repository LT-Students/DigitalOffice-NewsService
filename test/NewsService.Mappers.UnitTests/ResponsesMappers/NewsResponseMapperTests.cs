using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.NewsService.Mappers.Responses;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
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
        private Mock<ILogger<NewsResponseMapper>> _loggerMock;
        private Mock<IRequestClient<IGetUserDataRequest>> _userRequestClientMock;
        private Mock<IRequestClient<IGetDepartmentRequest>> _departmentRequestClientMock;
        private Mock<Response<IOperationResult<IGetUserDataResponse>>> _userBrokerResponseMock;
        private Mock<Response<IOperationResult<IGetDepartmentResponse>>> _departmentBrokerResponseMock;
        private Mock<IOperationResult<IGetUserDataResponse>> _userResponseMock;
        private Mock<IOperationResult<IGetDepartmentResponse>> _departmentResponseMock;

        private const string firstName = "Ivan";
        private const string lastName = "Ivanov";
        private const string middleName = "Ivanovich";
        private const string departmentName = "Name";

        private INewsResponseMapper _mapper;
        private NewsResponse _newsResponse;
        private DbNews _dbNews;
        private User _user;
        private Department _department;

        [SetUp]
        public void SetUp()
        {
            _user = new User { Id = Guid.NewGuid(), FullName = "Ivanov Ivan Ivanovich" };

            _department = new Department { Id = Guid.NewGuid(), Name = departmentName };

            _dbNews = new DbNews
            {
                Id = Guid.NewGuid(),
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "Pseudonym",
                AuthorId = _user.Id,
                SenderId = _user.Id,
                CreatedAt = DateTime.UtcNow,
                DepartmentId = _department.Id,
                IsActive = true
            };

            _newsResponse = new NewsResponse
            {
                Id = _dbNews.Id,
                Content = _dbNews.Content,
                Subject = _dbNews.Subject,
                Author = _user,
                Sender = _user,
                CreatedAt = _dbNews.CreatedAt,
                Department = _department,
                IsActive = _dbNews.IsActive
            };

            _userResponseMock = new Mock<IOperationResult<IGetUserDataResponse>>();

            _userResponseMock
                .Setup(x => x.Body.FirstName)
                .Returns(firstName);
            _userResponseMock
                .Setup(x => x.Body.LastName)
                .Returns(lastName);
            _userResponseMock
                .Setup(x => x.IsSuccess)
                .Returns(true);
            _userResponseMock
                .Setup(x => x.Body.MiddleName)
                .Returns(middleName);

            _userBrokerResponseMock = new Mock<Response<IOperationResult<IGetUserDataResponse>>>();
            _userBrokerResponseMock
                .Setup(x => x.Message)
                .Returns(_userResponseMock.Object);

            _userRequestClientMock = new Mock<IRequestClient<IGetUserDataRequest>>();

            _userRequestClientMock
                .Setup(x => x.GetResponse<IOperationResult<IGetUserDataResponse>>(
                    It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(_userBrokerResponseMock.Object));

            _departmentResponseMock = new Mock<IOperationResult<IGetDepartmentResponse>>();

            _departmentResponseMock
                .Setup(x => x.Body.Name)
                .Returns(departmentName);

            _departmentBrokerResponseMock = new Mock<Response<IOperationResult<IGetDepartmentResponse>>>();
            _departmentBrokerResponseMock
                .Setup(x => x.Message)
                .Returns(_departmentResponseMock.Object);

            _departmentResponseMock
                .Setup(x => x.IsSuccess)
                .Returns(true);

            _departmentRequestClientMock = new Mock<IRequestClient<IGetDepartmentRequest>>();

            _departmentRequestClientMock
                .Setup(x => x.GetResponse<IOperationResult<IGetDepartmentResponse>>(
                    It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(_departmentBrokerResponseMock.Object));

            _loggerMock = new Mock<ILogger<NewsResponseMapper>>();

            _mapper = new NewsResponseMapper(
                _userRequestClientMock.Object,
                _departmentRequestClientMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public void ShouldThrowBadRequestExceptionWhenDbNewsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }

        [Test]
        public void ShouldReturnNewsResponseModelWhenMappingValidDbNews()
        {
            SerializerAssert.AreEqual(_newsResponse, _mapper.Map(_dbNews));
        }

       [Test]
        public void ShouldMapWhenBadSenderIdTest()
        {
            _user.FullName = null;

            var responseMock = new Mock<IOperationResult<IGetUserDataResponse>>();
            responseMock
               .Setup(x => x.IsSuccess)
               .Returns(false);
            responseMock
               .Setup(x => x.Errors)
               .Returns(new List<string>() { "Not found senderId" });

            _userBrokerResponseMock
               .Setup(x => x.Message)
               .Returns(responseMock.Object);

            SerializerAssert.AreEqual(_newsResponse, _mapper.Map(_dbNews));
        }

        [Test]
        public void ShoulMapWhanBadDepartmentIdTest()
        {
            _department.Name = null;

            var responseMock = new Mock<IOperationResult<IGetDepartmentResponse>>();
            responseMock
               .Setup(x => x.IsSuccess)
               .Returns(false);
            responseMock
               .Setup(x => x.Errors)
               .Returns(new List<string>() { "Not found DepartmentId" });

            _departmentBrokerResponseMock
               .Setup(x => x.Message)
               .Returns(responseMock.Object);

            SerializerAssert.AreEqual(_newsResponse, _mapper.Map(_dbNews));
        }

        [Test]
        public void MapWithoutDepartmentIdTest()
        {
            _dbNews.DepartmentId = null;
            _newsResponse.Department = null;

            SerializerAssert.AreEqual(_newsResponse, _mapper.Map(_dbNews));
        }
    }
}
