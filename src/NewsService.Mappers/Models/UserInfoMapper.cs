using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class UserInfoMapper : IUserInfoMapper
  {
    public UserInfo Map(UserData author, ImageInfo image)
    {
      if (author == null)
      {
        return null;
      }

      return new UserInfo
      {
        Id = author.Id,
        FirstName = author.FirstName,
        MiddleName = author.MiddleName,
        LastName = author.LastName,
        Avatar = image
      };
    }
  }
}
