using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Business.Commands.Tags.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;

namespace LT.DigitalOffice.NewsService.Business.Commands.Tags
{
  public class FindTagCommand : IFindTagCommand
  {
    private readonly ITagRepository _repository;
    private readonly ITagInfoMapper _mapper;

    public FindTagCommand(
      ITagRepository repository,
      ITagInfoMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<FindResultResponse<TagInfo>> ExecuteAsync(FindTagFilter filter)
    {
      (List<DbTag> dbTagsList, int totalCount) = await _repository.FindAsync(filter);

      FindResultResponse<TagInfo> response = new();

      response.Body = _mapper.Map(dbTagsList);

      response.TotalCount = totalCount;

      return response;
    }
  }
}
