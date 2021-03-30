using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.NewsService.Business;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interface;
using LT.DigitalOffice.NewsService.Models.Broker.Requests;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Model;
using LT.DigitalOffice.NewsService.Models.Dto.ModelResponse;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.NewsService.Mappers.UnitTests.ResponsesMappers
{
    internal class NewsResponseMapperTests
    {
        private Mock<INewsResponseMapper> mapperMock;
        private Mock<INewsRepository> repositoryMock;
        private Mock<IRequestClient<IGetFIOUserRequest>> _requestclient;
        private NewsResponseMapper newsResponseMapper;

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
            mapperMock = new Mock<INewsResponseMapper>();
            repositoryMock = new Mock<INewsRepository>();

            _requestclient = new Mock<IRequestClient<IGetFIOUserRequest>>();

            _mapper = new NewsResponseMapper();//добавить client
        }

        [Test]
        public void ShouldThrowBadRequestExceptionWhenDbNewsIsNull()
        {
            Assert.Throws<BadRequestException>(() => newsResponseMapper.Map(null));
        }

        [Test]
        public void ShouldReturnNewsResponseModelWhenMappingValidDbNews()
        {
            var resultNewsModel = newsResponseMapper.Map(dbNews);

            repositoryMock.Setup(repository => repository.GetNewsInfoById(response.Id)).Returns(dbNews).Verifiable();
            mapperMock.Setup(mapper => mapper.Map(dbNews)).Returns(response).Verifiable();

            SerializerAssert.AreEqual(news, NewsResponseMapper.Execute(response.Id));
            repositoryMock.Verify();
            mapperMock.Verify();
        }
    }
}
