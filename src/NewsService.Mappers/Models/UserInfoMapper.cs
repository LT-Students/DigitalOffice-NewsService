using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class UserInfoMapper : IUserInfoMapper
  {
    public UserInfo Map(UserData user, ImageData avatarImage)
    {
      if (user is null)
      {
        return null;
      }

      return new UserInfo
      {
        Id = user.Id,
        FirstName = user.FirstName,
        MiddleName = user.MiddleName,
        LastName = user.LastName,
        Avatar = avatarImage
      };
    }
  }
}
