
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Moq;
using System.Text;
using Ensek.ErrorManagement.Exceptions;

namespace Ensek.Services.Tests;

[TestFixture]
public class FileValidatorTests
{
    private CsvFileValidationService<InvalidCsvException> _validator;
    private Mock<IFormFile> _fileMock;
    
    [SetUp]
    public void Setup()
    {
        _validator = new CsvFileValidationService<InvalidCsvException>();
        _fileMock = new Mock<IFormFile>();
    }

    [Test]
    public void ValidateFile_WithValidCsvFile_DoesNotThrowException()
    {
        // Arrange
        var content = "header1,header2,header3\n";
        var stream = CreateMemoryStream(content);

        _fileMock.Setup(f => f.FileName).Returns("test.csv");
        _fileMock.Setup(f => f.ContentType).Returns("text/csv");
        _fileMock.Setup(f => f.Length).Returns(stream.Length);
        _fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

        // Act & Assert
        Assert.DoesNotThrow(() => _validator.ValidateFile(_fileMock.Object));
    }

    [Test]
    public void ValidateFile_WithNullFile_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.Throws<InvalidCsvException>(() => 
            _validator.ValidateFile(null));
        Assert.That(ex.Message, Is.EqualTo("Attempted to validate null or empty file"));
    }

    [Test]
    public void ValidateFile_WithEmptyFile_ThrowsException()
    {
        // Arrange
        _fileMock.Setup(f => f.Length).Returns(0);

        // Act & Assert
        var ex = Assert.Throws<InvalidCsvException>(() => 
            _validator.ValidateFile(_fileMock.Object));
        Assert.That(ex.Message, Is.EqualTo("Attempted to validate null or empty file"));
    }

    [TestCase("test.txt")]
    [TestCase("test.xlsx")]
    [TestCase("test.dat")]
    public void ValidateFile_WithInvalidExtension_ThrowsException(string fileName)
    {
        // Arrange
        _fileMock.Setup(f => f.FileName).Returns(fileName);
        _fileMock.Setup(f => f.Length).Returns(100);

        // Act & Assert
        var ex = Assert.Throws<InvalidCsvException>(() => 
            _validator.ValidateFile(_fileMock.Object));
        Assert.That(ex.Message, Does.Contain("Invalid file extension"));
    }

    [TestCase("text/plain")]
    [TestCase("application/json")]
    [TestCase("application/xml")]
    public void ValidateFile_WithInvalidMimeType_ThrowsException(string mimeType)
    {
        // Arrange
        _fileMock.Setup(f => f.FileName).Returns("test.csv");
        _fileMock.Setup(f => f.ContentType).Returns(mimeType);
        _fileMock.Setup(f => f.Length).Returns(100);

        // Act & Assert
        var ex = Assert.Throws<InvalidCsvException>(() => 
            _validator.ValidateFile(_fileMock.Object));
        Assert.That(ex.Message, Does.Contain("Invalid MIME type"));
    }

    [Test]
    public void ValidateFile_WithEmptyContent_ThrowsException()
    {
        // Arrange
        var stream = CreateMemoryStream(string.Empty);

        _fileMock.Setup(f => f.FileName).Returns("test.csv");
        _fileMock.Setup(f => f.ContentType).Returns("text/csv");
        _fileMock.Setup(f => f.Length).Returns(stream.Length);
        _fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

        // Act & Assert
        var ex = Assert.Throws<InvalidCsvException>(() => 
            _validator.ValidateFile(_fileMock.Object));
        Assert.That(ex.Message, Is.EqualTo("Empty first line in CSV file"));
    }

    [TestCase("no|spaos|sponge")]
    [TestCase("invalid;csv;format")]
    public void ValidateFile_WithInvalidCsvStructure_ThrowsException(string content)
    {
        // Arrange
        var stream = CreateMemoryStream(content);

        _fileMock.Setup(f => f.FileName).Returns("test.csv");
        _fileMock.Setup(f => f.ContentType).Returns("text/csv");
        _fileMock.Setup(f => f.Length).Returns(stream.Length);
        _fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

        // Act & Assert
        var ex = Assert.Throws<InvalidCsvException>(() => 
            _validator.ValidateFile(_fileMock.Object));
        Assert.That(ex.Message, Is.EqualTo("Invalid CSV structure in first line"));
    }

    [Test]
    public void ValidateFile_WhenStreamThrowsException_ThrowsWrappedException()
    {
        // Arrange
        _fileMock.Setup(f => f.FileName).Returns("test.csv");
        _fileMock.Setup(f => f.ContentType).Returns("text/csv");
        _fileMock.Setup(f => f.Length).Returns(100);
        _fileMock.Setup(f => f.OpenReadStream()).Throws(new IOException("Test IO exception"));

        // Act & Assert
        Assert.Throws<IOException>(() => 
            _validator.ValidateFile(_fileMock.Object));
    }

    [Test]
    public void ValidateFile_WithDifferentExceptionType_ThrowsCorrectType()
    {
        // Arrange
        var validator = new CsvFileValidationService<ArgumentException>();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => 
            validator.ValidateFile(null));
        Assert.That(ex.Message, Is.EqualTo("Attempted to validate null or empty file"));
    }

    [Test]
    public void ValidateFile_WithMultipleValidMimeTypes_AcceptsAnyValidType()
    {
        // Arrange
        var validator = new CsvFileValidationService<InvalidCsvException>();

        var content = "header1,header2,header3\n";
        var stream = CreateMemoryStream(content);

        _fileMock.Setup(f => f.FileName).Returns("test.csv");
        _fileMock.Setup(f => f.ContentType).Returns("text/x-csv");
        _fileMock.Setup(f => f.Length).Returns(stream.Length);
        _fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

        // Act & Assert
        Assert.DoesNotThrow(() => validator.ValidateFile(_fileMock.Object));
    }

    private static MemoryStream CreateMemoryStream(string content)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}