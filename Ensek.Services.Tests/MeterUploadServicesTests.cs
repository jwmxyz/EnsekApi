using System.Linq.Expressions;
using Ensek.DataAccess.DbModels;
using Ensek.DataAccess.Repositories;
using Ensek.Services.Models;
using Ensek.Services.Models.CsvMappings;
using Ensek.Services.Models.DTO;
using Moq;
using NUnit.Framework;

namespace Ensek.Services.Tests;

[TestFixture]
public class MeterUploadServicesTests
{
    private Mock<ICsvServices> _csvServicesMock;
    private Mock<IRepository<MeterReading>> _meterReadingRepositoryMock;
    private Mock<IRepository<Account>> _accountRepositoryMock;
    private MeterUploadServices _service;

    [SetUp]
    public void Setup()
    {
        _csvServicesMock = new Mock<ICsvServices>();
        _meterReadingRepositoryMock = new Mock<IRepository<MeterReading>>();
        _accountRepositoryMock = new Mock<IRepository<Account>>();
        
        _service = new MeterUploadServices(
            _csvServicesMock.Object,
            _meterReadingRepositoryMock.Object,
            _accountRepositoryMock.Object);
    }

    [Test]
    public async Task Upload_ValidRecords_InsertsRecords()
    {
        // Arrange
        var stream = new MemoryStream();
        var records = new List<MeterReadingRecord>
        {
            new() { AccountId = 1, ReadingValue = 100, ReadingDateTime = DateTime.Now },
            new() { AccountId = 2, ReadingValue = 200, ReadingDateTime = DateTime.Now }
        };
        
        var deduplicatedRecords = new CsvParsingResults<MeterReadingRecord>
        {
            ValidRecords = records,
            InvalidRecords = new List<string>()
        };

        _csvServicesMock.Setup(x => x.Read<MeterReadingRecord, MeterReadingMapping>(It.IsAny<Stream>(), It.IsAny<bool>()))
            .Returns(deduplicatedRecords);
        
        _csvServicesMock.Setup(x => x.Deduplicate(
                It.IsAny<CsvParsingResults<MeterReadingRecord>>(),
                It.IsAny<Func<MeterReadingRecord, object>>()))
            .Returns(deduplicatedRecords);

        _accountRepositoryMock.Setup(x => x.GetAll(It.IsAny<Expression<Func<Account, bool>>>()))
            .ReturnsAsync(new List<Account> { new() { Id = 1 }, new() { Id = 2 } });

        _meterReadingRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(new List<MeterReading>());

        // Act
        var result = await _service.Upload(stream);

        // Assert
        Assert.That(result.ValidRecords.Count, Is.EqualTo(2));
        _meterReadingRepositoryMock.Verify(x => x.Insert(It.Is<IEnumerable<MeterReading>>(
            readings => readings.Count() == 2)), Times.Once);
    }

    [Test]
    public async Task Upload_InvalidAccounts_RemovesInvalidRecords()
    {
        // Arrange
        var stream = new MemoryStream();
        var records = new List<MeterReadingRecord>
        {
            new() { AccountId = 1, ReadingValue = 100, ReadingDateTime = DateTime.Now },
            new() { AccountId = 999, ReadingValue = 200, ReadingDateTime = DateTime.Now } // Invalid account
        };
        
        var deduplicatedRecords = new CsvParsingResults<MeterReadingRecord>
        {
            ValidRecords = records.ToList(),
            InvalidRecords = new List<string>()
        };

        _csvServicesMock.Setup(x => x.Read<MeterReadingRecord, MeterReadingMapping>(It.IsAny<Stream>(), It.IsAny<bool>()))
            .Returns(deduplicatedRecords);
        
        _csvServicesMock.Setup(x => x.Deduplicate(
                It.IsAny<CsvParsingResults<MeterReadingRecord>>(),
                It.IsAny<Func<MeterReadingRecord, object>>()))
            .Returns(deduplicatedRecords);

        _accountRepositoryMock.Setup(x => x.GetAll(It.IsAny<Expression<Func<Account, bool>>>()))
            .ReturnsAsync(new List<Account> { new() { Id = 1 } }); 

        _meterReadingRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(new List<MeterReading>());

        // Act
        var result = await _service.Upload(stream);

        // Assert
        Assert.That(result.ValidRecords.Count, Is.EqualTo(1));
        Assert.That(result.InvalidRecords.Count, Is.EqualTo(1));
        Assert.That(result.InvalidRecords[0], Does.Contain("Invalid account number"));
    }

    [Test]
    public async Task Upload_DuplicateExistingEntries_RemovesDuplicates()
    {
        // Arrange
        var stream = new MemoryStream();
        var dateTime = DateTime.Now;
        var records = new List<MeterReadingRecord>
        {
            new() { AccountId = 1, ReadingValue = 100, ReadingDateTime = dateTime },
            new() { AccountId = 2, ReadingValue = 200, ReadingDateTime = dateTime }
        };
        
        var deduplicatedRecords = new CsvParsingResults<MeterReadingRecord>
        {
            ValidRecords = records.ToList(),
            InvalidRecords = new List<string>()
        };

        var existingReadings = new List<MeterReading>
        {
            new() { AccountId = 1, ReadingValue = 100, ReadingDateTime = dateTime } 
        };

        _csvServicesMock.Setup(x => x.Read<MeterReadingRecord, MeterReadingMapping>(It.IsAny<Stream>(), It.IsAny<bool>()))
            .Returns(deduplicatedRecords);
        
        _csvServicesMock.Setup(x => x.Deduplicate(
                It.IsAny<CsvParsingResults<MeterReadingRecord>>(),
                It.IsAny<Func<MeterReadingRecord, object>>()))
            .Returns(deduplicatedRecords);

        _accountRepositoryMock.Setup(x => x.GetAll(It.IsAny<Expression<Func<Account, bool>>>()))
            .ReturnsAsync(new List<Account> { new() { Id = 1 }, new() { Id = 2 } });

        _meterReadingRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(existingReadings);

        // Act
        var result = await _service.Upload(stream);

        // Assert
        Assert.That(result.ValidRecords.Count, Is.EqualTo(1));
        Assert.That(result.InvalidRecords.Count, Is.EqualTo(1));
        Assert.That(result.InvalidRecords[0], Does.Contain("entry already exists"));
        _meterReadingRepositoryMock.Verify(x => x.Insert(It.Is<IEnumerable<MeterReading>>(
            readings => readings.Count() == 1)), Times.Once);
    }
}