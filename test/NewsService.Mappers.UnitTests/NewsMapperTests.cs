using LT.DigitalOffice.Kernel.UnitTestLibrary;
using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.NewsService.Mappers.UnitTests
{
    class NewsMapperTests
    {
        private IMapper<CreateNewsRequest, DbNews> mapper;
        private CreateNewsRequest createNewsRequest;
        private DbNews expectedDbNews;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mapper = new NewsMapper();

            createNewsRequest = new CreateNewsRequest
            {
                Content = "Content",
                Subject = "Subject",
                AuthorName = "AuthorName",
                AuthorId = Guid.NewGuid(),
                SenderId = Guid.NewGuid()
            };
            expectedDbNews = new DbNews
            {
                Content = "Content",
                Subject = "Subject",
                AuthorName = "AuthorName",
                AuthorId = createNewsRequest.AuthorId,
                SenderId = createNewsRequest.SenderId,
                IsActive = true
            };

        }

        #region CreateNewsRequest to DbNews
        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => mapper.Map(null));
        }

        [Test]
        public void ShouldReturnRightModelWhenRequestIsMapped()
        {
            var dbNews = mapper.Map(createNewsRequest);
            expectedDbNews.Id = dbNews.Id;
            expectedDbNews.CreatedAt = dbNews.CreatedAt;

            Assert.IsInstanceOf<Guid>(dbNews.Id);
            SerializerAssert.AreEqual(expectedDbNews, dbNews);
        }
        #endregion
    }
}
