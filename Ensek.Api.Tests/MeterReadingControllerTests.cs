using Ensek.Api.Controllers;
using Ensek.Api.Factory;
using Ensek.ErrorManagement.Exceptions;
using Ensek.Services;
using Ensek.Services.Models;
using Ensek.Services.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Ensek.Api.Tests;

[TestFixture]
public class MeterReadingControllerTests
{
    private Mock<ILogger<MeterReadingController>> _loggerMock;
    private Mock<IResponseFactory> _responseFactoryMock;
    private Mock<IUploadServices<MeterReadingRecord>> _meterUploadServicesMock;
    private Mock<IFileValidationService<InvalidCsvException>> _csvFileValidation;
    private MeterReadingController _controller;

    [SetUp]
    public void Setup()
    {
        _responseFactoryMock = new Mock<IResponseFactory>();
        _meterUploadServicesMock = new Mock<IUploadServices<MeterReadingRecord>>();
        _csvFileValidation = new Mock<IFileValidationService<InvalidCsvException>>();
        
        _controller = new MeterReadingController(
            _responseFactoryMock.Object,
            _meterUploadServicesMock.Object,
            _csvFileValidation.Object);
    }

    [Test]
    public async Task Upload_WithValidFile_ReturnsSuccessResponse()
    {
        // Arrange
        var expectedResult = new CsvParsingResults<MeterReadingRecord> { ValidRecords = [new MeterReadingRecord()] };
        var expectedResponse = new OkObjectResult(expectedResult);

        var fileMock = new Mock<IFormFile>();
        var content = "test content";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        stream.Position = 0;

        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

        _meterUploadServicesMock
            .Setup(s => s.Upload(It.IsAny<Stream>()))
            .ReturnsAsync(expectedResult);

        _responseFactoryMock
            .Setup(f => f.CreateResponse(expectedResult))
            .Returns(expectedResponse);
        
        // Act
        var result = await _controller.Upload(fileMock.Object);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResponse));
        _meterUploadServicesMock.Verify(s => s.Upload(It.IsAny<Stream>()), Times.Once);
        _responseFactoryMock.Verify(f => f.CreateResponse(expectedResult), Times.Once);
    }

    [Test]
    public async Task Upload_WhenServiceThrowsException_ReturnsErrorResponse()
    {
        // Arrange
        var expectedException = new Exception("Upload failed");
        var expectedResponse = new BadRequestObjectResult("Error occurred");

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        _meterUploadServicesMock
            .Setup(s => s.Upload(It.IsAny<Stream>()))
            .ThrowsAsync(expectedException);

        _responseFactoryMock
            .Setup(f => f.CreateResponse(expectedException))
            .Returns(expectedResponse);

        // Act
        var result = await _controller.Upload(fileMock.Object);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResponse));
        _responseFactoryMock.Verify(f => f.CreateResponse(expectedException), Times.Once);
    }
}