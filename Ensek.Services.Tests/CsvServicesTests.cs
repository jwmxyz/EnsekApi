using System.Text;
using CsvHelper.Configuration;
using Ensek.Services.Models;
using NUnit.Framework;

namespace Ensek.Services.Tests;

[TestFixture]
public class CsvServicesTests
{
    private CsvServices _csvServices;

    [SetUp]
    public void Setup()
    {
        _csvServices = new CsvServices();
    }

    public class TestRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public sealed class TestRecordMap : ClassMap<TestRecord>
    {
        public TestRecordMap()
        {
            Map(m => m.Id);
            Map(m => m.Name);
            Map(m => m.Date);
        }
    }

    private Stream CreateCsvStream(string content)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    [Test]
    public void Read_ValidCsvWithHeader_ReturnsValidRecords()
    {
        // Arrange
        var csvContent = @"Id,Name,Date
1,Test1,2024-01-01
2,Test2,2024-01-02";
        
        using var stream = CreateCsvStream(csvContent);

        // Act
        var result = _csvServices.Read<TestRecord, TestRecordMap>(stream);

        // Assert
        Assert.That(result.ValidRecords.Count, Is.EqualTo(2));
        Assert.That(result.InvalidRecords.Count, Is.EqualTo(0));
        Assert.That(result.ValidRecords[0].Id, Is.EqualTo(1));
        Assert.That(result.ValidRecords[0].Name, Is.EqualTo("Test1"));
        Assert.That(result.ValidRecords[0].Date, Is.EqualTo(new DateTime(2024, 1, 1)));
    }

    [Test]
    public void Read_ValidCsvWithoutHeader_ReturnsValidRecords()
    {
        // Arrange
        var csvContent = @"1,Test1,2024-01-01
2,Test2,2024-01-02";
        
        using var stream = CreateCsvStream(csvContent);

        // Act
        var result = _csvServices.Read<TestRecord, TestRecordMap>(stream, false);

        // Assert
        Assert.That(result.ValidRecords.Count, Is.EqualTo(2));
        Assert.That(result.InvalidRecords.Count, Is.EqualTo(0));
    }

    [Test]
    public void Read_InvalidDataFormat_ReturnsErrorMessages()
    {
        // Arrange
        var csvContent = @"Id,Name,Date
1,Test1,2024-01-01
invalid,Test2,not-a-date";
        
        using var stream = CreateCsvStream(csvContent);

        // Act
        var result = _csvServices.Read<TestRecord, TestRecordMap>(stream);

        // Assert
        Assert.That(result.InvalidRecords.Count, Is.EqualTo(1));
        Assert.That(result.InvalidRecords[0], Does.Contain("Error at row"));
    }

    [Test]
    public void Read_EmptyCsv_ReturnsEmptyResults()
    {
        // Arrange
        var csvContent = "Id,Name,Date";
        using var stream = CreateCsvStream(csvContent);

        // Act
        var result = _csvServices.Read<TestRecord, TestRecordMap>(stream);

        // Assert
        Assert.That(result.ValidRecords.Count, Is.EqualTo(0));
        Assert.That(result.InvalidRecords.Count, Is.EqualTo(0));
    }

    [Test]
    public void Deduplicate_NoDuplicates_ReturnsOriginalRecords()
    {
        // Arrange
        var records = new CsvParsingResults<TestRecord>
        {
            ValidRecords = new List<TestRecord>
            {
                new() { Id = 1, Name = "Test1", Date = DateTime.Parse("2024-01-01") },
                new() { Id = 2, Name = "Test2", Date = DateTime.Parse("2024-01-02") }
            },
            InvalidRecords = new List<string>()
        };

        // Act
        var result = _csvServices.Deduplicate(records, r => new { r.Id, r.Name });

        // Assert
        Assert.That(result.ValidRecords.Count, Is.EqualTo(2));
        Assert.That(result.InvalidRecords.Count, Is.EqualTo(0));
    }

    [Test]
    public void Deduplicate_WithDuplicates_RemovesDuplicatesAndAddsToInvalidRecords()
    {
        // Arrange
        var records = new CsvParsingResults<TestRecord>
        {
            ValidRecords = new List<TestRecord>
            {
                new() { Id = 1, Name = "Test1", Date = DateTime.Parse("2024-01-01") },
                new() { Id = 1, Name = "Test1", Date = DateTime.Parse("2024-01-02") },
                new() { Id = 2, Name = "Test2", Date = DateTime.Parse("2024-01-03") }
            },
            InvalidRecords = new List<string>()
        };

        // Act
        var result = _csvServices.Deduplicate(records, r => new { r.Id, r.Name });

        // Assert
        Assert.That(result.ValidRecords.Count, Is.EqualTo(2));
        Assert.That(result.InvalidRecords.Count, Is.EqualTo(1));
        Assert.That(result.ValidRecords.First().Date, Is.EqualTo(DateTime.Parse("2024-01-01")));
    }

    [Test]
    public void Deduplicate_MultipleGroupsWithDuplicates_HandlesCorrectly()
    {
        // Arrange
        var records = new CsvParsingResults<TestRecord>
        {
            ValidRecords = new List<TestRecord>
            {
                new() { Id = 1, Name = "Test1", Date = DateTime.Parse("2024-01-01") },
                new() { Id = 1, Name = "Test1", Date = DateTime.Parse("2024-01-02") },
                new() { Id = 2, Name = "Test2", Date = DateTime.Parse("2024-01-03") },
                new() { Id = 2, Name = "Test2", Date = DateTime.Parse("2024-01-04") },
                new() { Id = 3, Name = "Test3", Date = DateTime.Parse("2024-01-05") }
            },
            InvalidRecords = new List<string>()
        };

        // Act
        var result = _csvServices.Deduplicate(records, r => new { r.Id, r.Name });

        // Assert
        Assert.That(result.ValidRecords.Count, Is.EqualTo(3));
        Assert.That(result.InvalidRecords.Count, Is.EqualTo(2));
    }

    [Test]
    public void Deduplicate_EmptyRecords_ReturnsEmptyResults()
    {
        // Arrange
        var records = new CsvParsingResults<TestRecord>
        {
            ValidRecords = new List<TestRecord>(),
            InvalidRecords = new List<string>()
        };

        // Act
        var result = _csvServices.Deduplicate(records, r => new { r.Id, r.Name });

        // Assert
        Assert.That(result.ValidRecords.Count, Is.EqualTo(0));
        Assert.That(result.InvalidRecords.Count, Is.EqualTo(0));
    }
}