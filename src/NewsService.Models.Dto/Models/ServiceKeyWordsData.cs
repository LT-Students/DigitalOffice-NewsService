using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.NewsService.Models.Dto.Models
{
  public class ServiceKeyWordsData
  {
    string TableName { get; }
    List<string> KeyWords { get; }

    public ServiceKeyWordsData(string tableName, List<string> keyWords)
    {
      TableName = tableName;
      KeyWords = keyWords;
    }
   }
}
