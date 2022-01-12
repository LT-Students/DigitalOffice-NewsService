using System;
using LT.DigitalOffice.NewsService.Models.Dto.Configuration;

namespace LT.DigitalOffice.NewsService
{
  [AttributeUsage(AttributeTargets.Property)]
  public class KeyWordAttribute : Attribute
  {
    public ServiceEndpoints[] Endpoints { get; }

    public KeyWordAttribute(params ServiceEndpoints[] endpoints)
    {
      Endpoints = endpoints;
    }
  }
}
