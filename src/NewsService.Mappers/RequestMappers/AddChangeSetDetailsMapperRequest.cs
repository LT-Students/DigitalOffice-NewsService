using LT.DigitalOffice.NewsService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;

namespace LT.DigitalOffice.NewsService.Mappers.RequestMappers
{
    public class AddChangeSetDetailsMapperRequest : IAddChangeSetDetailsMapperRequest
    {
        public DbChangeSetDetails Map(Operation<DbNews> patchData)
        {
            return new DbChangeSetDetails
            {
                Id = Guid.NewGuid(),
                FieldName = patchData.path,
                Value = patchData.value.ToString()
            };
        }
    }
}
