using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using NUnit.Framework;
using System;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models;

namespace LT.DigitalOffice.NewsService.Mappers.UnitTests.ModelMappers
{
    class NewsMapperTests
    {
        private IDbNewsMapper _mapper;
        private News _newsRequest;
        private DbNews _expectedDbNews;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new DbNewsMapper();

            _newsRequest = new News
            {
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "AuthorName",
                AuthorId = Guid.NewGuid(),
                SenderId = Guid.NewGuid()
            };

            _expectedDbNews = new DbNews
            {
                Content = "Content",
                Subject = "Subject",
                Pseudonym = "AuthorName",
                AuthorId = _newsRequest.AuthorId,
                SenderId = _newsRequest.SenderId,
                IsActive = true
            };
        }

        #region NewsRequest to DbNews

        [Test]
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
        }

        #endregion
    }
}
