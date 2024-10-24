using Ensek.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ensek.Api.Factory;

public interface IResponseFactory
{
    /// <summary>
    /// This provide the correct IActionresult for the results of a CsvParsing request.
    /// </summary>
    /// <param name="results">the results to determine a request for</param>
    /// <typeparam name="T">The type of csvParsingResult</typeparam>
    /// <returns>
    /// An <see cref="IActionResult"/> representing the response to the client:
    /// <list type="bullet">
    /// <item>
    /// <description>Returns a Bad Request response if there are invalid records and no valid records.</description>
    /// </item>
    /// <item>
    /// <description>Returns a Conflict response if there are both valid and invalid records.</description>
    /// </item>
    /// <item>
    /// <description>Returns an OK response if there are only valid records.</description>
    /// </item>
    /// </list>
    /// </returns>
    IActionResult CreateResponse<T>(CsvParsingResults<T> results);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IActionResult CreateResponse<T>(T exception) where T : Exception;
}