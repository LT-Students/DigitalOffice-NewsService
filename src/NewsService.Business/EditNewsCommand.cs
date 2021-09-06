using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Net;

namespace LT.DigitalOffice.NewsService.Business
{
    public class EditNewsCommand : IEditNewsCommand
    {

        private readonly INewsRepository _repository;
        private readonly IPatchNewsMapper _mapper;
        private readonly IEditNewsValidator _validator;
        private readonly IAccessValidator _accessValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditNewsCommand(
            INewsRepository repository,
            IPatchNewsMapper mapper,
            IEditNewsValidator validator,
            IAccessValidator accessValidator,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
            _accessValidator = accessValidator;
            _httpContextAccessor = httpContextAccessor;
        }

        public OperationResultResponse<bool> Execute(Guid newsId, JsonPatchDocument<EditNewsRequest> request)
        {
            if (!(_accessValidator.HasRights(Rights.AddEditRemoveNews)))
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                return new OperationResultResponse<bool>
                {
                    Status = OperationResultStatusType.Failed,
                    Errors = new() { "Not enough rights." }
                };
            }

            if (!_validator.ValidateCustom(request, out List<string> errors))
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return new OperationResultResponse<bool>
                {
                    Status = OperationResultStatusType.Failed,
                    Errors = errors
                };
            }

            OperationResultResponse<bool> response = new();

            JsonPatchDocument<DbNews> dbRequest = _mapper.Map(request);

            response.Body = _repository.EditNews(newsId, dbRequest);

            if (!response.Body)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                response.Errors.Add("Bad request");
                response.Status = OperationResultStatusType.Failed;
                return response;
            }

            response.Status = OperationResultStatusType.FullSuccess;
            return response;
        }
    }
}
