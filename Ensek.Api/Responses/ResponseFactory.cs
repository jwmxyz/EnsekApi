using Ensek.Api.Responses;
using Ensek.ErrorManagement.Exceptions;
using Ensek.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ensek.Api.Factory;

public class ResponseFactory : IResponseFactory
{
    public IActionResult CreateResponse<T>(CsvParsingResults<T> results)
    {
        if (results == null)
        {
            return CreateBadRequestResponse();
        }

        if (results.InvalidRecords.Count > 0 && results.ValidRecords.Count == 0)
        {
            return CreateBadRequestResponse(results.InvalidRecords);
        }

        if (results.ValidRecords.Count > 0 && results.InvalidRecords.Count > 0)
        {
            return CreateConflictResponse(results);
        }

        return CreateOkResponse(results);
    }

    public IActionResult CreateResponse<T>(T exception) where T : Exception
    {
        return exception switch
        {
            InvalidCsvException => new BadRequestObjectResult(
                new EnsekStandardResult(null, true, exception.Message))
            {
                StatusCode = StatusCodes.Status400BadRequest
            },
            _ => new ObjectResult(
                new EnsekStandardResult(null, true, exception.Message))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }

    private static IActionResult CreateBadRequestResponse(List<string> errors = null)
    {
        return new BadRequestObjectResult(new EnsekStandardResult(null, true, errors));
    }

    private static IActionResult CreateConflictResponse<T>(CsvParsingResults<T> results)
    {
        return new ConflictObjectResult(new EnsekStandardResult(results, true, results.InvalidRecords))
        {
            StatusCode = StatusCodes.Status207MultiStatus
        };
    }

    private static IActionResult CreateOkResponse<T>(CsvParsingResults<T> results)
    {
        return new OkObjectResult(new EnsekStandardResult(results));
    }
}