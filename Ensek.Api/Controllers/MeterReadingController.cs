using System.ComponentModel.DataAnnotations;
using Ensek.Api.Factory;
using Ensek.DataAccess.DbModels;
using Ensek.ErrorManagement.Exceptions;
using Ensek.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ensek.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MeterReadingController(
    ILogger<MeterReadingController> logger,
    IResponseFactory responseFactory,
    IUploadServices<MeterReadingRecord> meterUploadServices,
    IFileValidationService<InvalidCsvException> csvFileValidationService)
    : ControllerBase
{
    private readonly ILogger<MeterReadingController> _logger = logger;

    [HttpPost]
    [Route("/meter-reading-uploads")]
    public async Task<IActionResult> Upload([Required] IFormFile csv)
    {
        csvFileValidationService.ValidateFile(csv);
        
        try
        {
            var result = await meterUploadServices.Upload(csv.OpenReadStream());
            return responseFactory.CreateResponse(result);
        }
        catch (Exception e)
        {
            return responseFactory.CreateResponse(e);
        }
    }
}