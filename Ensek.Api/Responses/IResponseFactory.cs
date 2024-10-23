using Ensek.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ensek.Api.Factory;

public interface IResponseFactory
{
    IActionResult CreateResponse<T>(CsvParsingResults<T> results);
    IActionResult CreateResponse<T>(T exception) where T : Exception;
}