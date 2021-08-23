using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Validation.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.NewsService.Business
{
    public class CreateNewsCommand : ICreateNewsCommand
    {
        private readonly INewsRepository _repository;
        private readonly IDbNewsMapper _mapper;
        private readonly INewsValidator _validator;
        private readonly IAccessValidator _accessValidator;
        private readonly IRequestClient<ICheckDepartmentsExistence> _rcCheckDepartmentsExistence;
        private readonly ILogger<CreateNewsCommand> _logger;

        private List<Guid> CheckDepartmentExistence(Guid? departmentId, List<string> errors)
        {
            string errorMessage = "Failed to check the existing department.";
            string logMessage = "Department not found. {departmentIds}";

            try
            {
                var response = _rcCheckDepartmentsExistence.GetResponse<IOperationResult<ICheckDepartmentsExistence>>(
                    ICheckDepartmentsExistence.CreateObj(new List<Guid> { departmentId.Value })).Result;
                if (response.Message.IsSuccess)
                {
                    if (!response.Message.Body.DepartmentIds.Any())
                    {
                        errors.Add("Department Id does not exist");
                    }
                    return response.Message.Body.DepartmentIds;
                }

                _logger.LogWarning($"Can not find {departmentId} with this Ids '{departmentId}': " +
                    $"{Environment.NewLine}{string.Join('\n', response.Message.Errors)}");
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, logMessage);
            }

            errors.Add(errorMessage);
            return new List<Guid>();
        }

        public CreateNewsCommand(
            INewsRepository repository,
            IDbNewsMapper mapper,
            INewsValidator validator,
            IAccessValidator accessValidator,
            IRequestClient<ICheckDepartmentsExistence> rcCheckDepartmentsExistence,
            ILogger<CreateNewsCommand> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
            _accessValidator = accessValidator;
            _rcCheckDepartmentsExistence = rcCheckDepartmentsExistence;
            _logger = logger;
        }

        public OperationResultResponse<Guid> Execute(News request)
        {
            if (!(_accessValidator.IsAdmin() ||
                 _accessValidator.HasRights(Rights.AddEditRemoveNews)))
            {
                throw new ForbiddenException("Not enough rights.");
            }

            OperationResultResponse<Guid> response = new();

            _validator.ValidateAndThrowCustom(request);

            List<Guid> existDepartments = CheckDepartmentExistence(request.DepartmentId, response.Errors);

            var news = _mapper.Map(request, existDepartments);

            return new OperationResultResponse<Guid>
            {
                Body = _repository.CreateNews(news),
                Status = OperationResultStatusType.FullSuccess,
                Errors = response.Errors
            };
        }
    }
}
