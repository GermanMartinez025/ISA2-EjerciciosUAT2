using ControllersInterfaces.Importers;
using Microsoft.AspNetCore.Mvc;
using ModelsAPI;
using ModelsAPI.Importers;
using WebAPI.Filters;

namespace WebAPI.APIs;

[ApiController]
[Route("api/imports")]
[ExceptionFilter]
public class ImporterAPI : ControllerBase
{
    
        private readonly IImporterController _importController;

        public ImporterAPI(IImporterController importController)
        {
            _importController = importController;
        }

        [HttpGet]
        public GenericResponse GetAvailableImporters()
        {
            var response = _importController.GetAllImporters();
            var genericResponse = new GenericResponse();
            genericResponse.ExecutionSuccessful = true;
            genericResponse.Message = "Available importers retrieved successfully";
            genericResponse.Data = response;
            return genericResponse;
        }

    }
