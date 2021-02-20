using LT.DigitalOffice.NewsService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace LT.DigitalOffice.NewsService.Mappers.RequestMappers
{
    public class AddNewsChangesHistoryMapperRequest : IAddNewsChangesHistoryMapperRequest
    {
        private readonly IAddChangeSetDetailsMapperRequest _changeSetDetailsMapper;

        public AddNewsChangesHistoryMapperRequest([FromServices] IAddChangeSetDetailsMapperRequest mapper)
        {
            _changeSetDetailsMapper = mapper;
        }

        public DbNewsChangesHistory Map(JsonPatchDocument<DbNews> patch)
        {
            if (patch == null)
            {
                throw new ArgumentNullException(nameof(patch));
            }

            var newsHistoryId = Guid.NewGuid();

            return new DbNewsChangesHistory
            {
                Id = newsHistoryId,
                ChangedAt = DateTime.Now,
                ChangeSetDetails = patch.Operations.Select(x => GetListChangeSetDetails(x, newsHistoryId)).ToList()
            };
        }

        private DbChangeSetDetails GetListChangeSetDetails(Operation<DbNews> patchData, Guid newsHistoryId)
        {
            DbChangeSetDetails dbChanges = _changeSetDetailsMapper.Map(patchData);

            dbChanges.NewsHistoryId = newsHistoryId;

            return dbChanges;
        }
    }
}