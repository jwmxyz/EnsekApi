using Ensek.Api.Factory;
using Ensek.ErrorManagement.Exceptions;
using Ensek.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ensek.Api.Responses;

public class ResponseFactory : IResponseFactory
{
    /// <see cref="IResponseFactory.CreateResponse{T}(CsvParsingResults{T})"/>
    public IActionResult CreateResponse<T>(CsvParsingResults<T> results)
    {
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

    /// <see cref="IResponseFactory.CreateResponse{T}({T})"/>
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

    private static IActionResult CreateBadRequestResponse(IEnumerable<string>? errors = null)
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