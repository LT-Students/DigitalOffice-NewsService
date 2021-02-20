using FluentValidation;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.NewsService.Business.UnitTests
{
    internal class EditNewsCommandTests
    {
        private IEditNewsCommand _command;

        private Mock<INewsRepository> _repository;
        private Mock<IAddNewsChangesHistoryMapperRequest> _mapper;
        private Mock<IValidator<JsonPatchDocument<DbNews>>> _validator;

        private JsonPatchDocument<DbNews> _editNewsRequest;
        private Guid _userId;
        private Guid _newId;
        private DbNews _dbNews;
        private DbNewsChangesHistory _dbNewsChangesHistory;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<INewsRepository>();
            _mapper = new Mock<IAddNewsChangesHistoryMapperRequest>();
            _validator = new Mock<IValidator<JsonPatchDocument<DbNews>>>();

            _userId = Guid.NewGuid();
            _newId = Guid.NewGuid();

            _dbNews = new DbNews
            {
                Id = _newId,
                AuthorId = Guid.NewGuid(),
                SenderId = Guid.NewGuid(),
                Content = "Message content",
                Subject = "Message subject",
                AuthorName = "Author"
            };

            _dbNewsChangesHistory = new DbNewsChangesHistory
            {
                Id = Guid.NewGuid(),
                NewsId = _newId,
                ChangedBy = _userId,
                ChangedAt = DateTime.Now
            };

            _editNewsRequest = new JsonPatchDocument<DbNews>().Add(x => x.Subject, "Message subject");

            _command = new EditNewsCommand(_repository.Object, _mapper.Object, _validator.Object);
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenNewsIdNotFound()
        {
            _repository
                .Setup(x => x.GetNew(_newId))
                .Throws(new ArgumentException());

            Assert.Throws<ArgumentException>(() => _command.Execute(_userId, _newId, _editNewsRequest));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenPatchIsNull()
        {
            JsonPatchDocument<DbNews> editNewsRequest = null;

            _repository
                .Setup(x => x.GetNew(_newId))
                .Returns(_dbNews)
                .Verifiable();

            _mapper
                .Setup(x => x.Map(editNewsRequest))
                .Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => _command.Execute(_userId, _newId, editNewsRequest));
            _repository.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenDbNewsIsNullInTheRepository()
        {
            DbNews dbNews = null;

            _repository
                .Setup(x => x.GetNew(_newId))
                .Returns(dbNews)
                .Verifiable();

            _mapper
                .Setup(x => x.Map(_editNewsRequest))
                .Returns(_dbNewsChangesHistory)
                .Verifiable();

            Assert.Throws<ArgumentNullException>(() => _command.Execute(_userId, _newId, _editNewsRequest));
            _repository.Verify();
            _mapper.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenDbNewsIsNull()
        {
            _repository
                .Setup(x => x.GetNew(_newId))
                .Returns(_dbNews)
                .Verifiable();

            _mapper
                .Setup(x => x.Map(_editNewsRequest))
                .Returns(_dbNewsChangesHistory)
                .Verifiable();

            _repository
                .Setup(x => x.EditNews(It.IsAny<DbNews>()))
                .Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => _command.Execute(_userId, _newId, _editNewsRequest));
            _repository.Verify();
            _mapper.Verify();
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenDbNewsChangesHistoryIsNull()
        {
            _repository
                .Setup(x => x.GetNew(_newId))
                .Returns(_dbNews)
                .Verifiable();

            _mapper
                .Setup(x => x.Map(_editNewsRequest))
                .Returns(_dbNewsChangesHistory)
                .Verifiable();

            _repository
                .Setup(x => x.CreateNewsHistory(It.IsAny<DbNewsChangesHistory>(), _newId))
                .Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => _command.Execute(_userId, _newId, _editNewsRequest));
            _repository.Verify();
            _mapper.Verify();
        }

        [Test]
        public void ShouldEditNewsSuccefull()
        {
            _repository
                .Setup(x => x.GetNew(_newId))
                .Returns(_dbNews)
                .Verifiable();

            _mapper
                .Setup(x => x.Map(_editNewsRequest))
                .Returns(_dbNewsChangesHistory)
                .Verifiable();

            Assert.DoesNotThrow(() => _command.Execute(_userId, _newId, _editNewsRequest));
            _repository.Verify();
            _mapper.Verify();
        }
    }
}