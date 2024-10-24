using System.ComponentModel.DataAnnotations;
using Ensek.Api.Factory;
using Ensek.ErrorManagement.Exceptions;
using Ensek.Services;
using Ensek.Services.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Ensek.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MeterReadingController(
    IResponseFactory responseFactory,
    IUploadServices<MeterReadingRecord> meterUploadServices,
    IFileValidationService<InvalidCsvException> csvFileValidationService)
    : ControllerBase
{

    /// <summary>
    /// API method that will process an incoming .csv file.
    /// </summary>
    /// <param name="csv">the incoming .csv file</param>
    /// <returns>A response based on the outcome</returns>
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