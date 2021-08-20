using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using NUnit.Framework;
using System;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Mappers.UnitTests.ModelMappers
{
    class NewsMapperTests
    {
        private IDbNewsMapper _mapper;
        private News _newsRequest;
        private DbNews _expectedDbNews;
        private Mock<IHttpContextAccessor> _accessorMock;

        private Guid _userId = Guid.NewGuid();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _accessorMock = new();
            IDictionary<object, object> _items = new Dictionary<object, object>();
            _items.Add("UserId", _userId);

            _accessorMock
                .Setup(x => x.HttpContext.Items)
                .Returns(_items);

            _mapper = new DbNewsMapper(_accessorMock.Object);

            _newsRequest = new News
            {
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "AuthorName",
                AuthorId = Guid.NewGuid()
            };

            _expectedDbNews = new DbNews
            {
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "AuthorName",
                AuthorId = _newsRequest.AuthorId,
                IsActive = true
            };
        }

        #region NewsRequest to DbNews

        /*[Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }

        [Test]
        public void ShouldReturnRightModelWhenRequestIsMapped()
        {
            var dbNews = _mapper.Map(_newsRequest);
            _expectedDbNews.Id = dbNews.Id;
            _expectedDbNews.CreatedBy = dbNews.CreatedBy;
            _expectedDbNews.CreatedAtUtc = dbNews.CreatedAtUtc;

            Assert.IsInstanceOf<Guid>(dbNews.Id);
            SerializerAssert.AreEqual(_expectedDbNews, dbNews);
        }*/

        #endregion
    }
}
