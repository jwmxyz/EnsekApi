using Ensek.Services.Models.DTO;
using NUnit.Framework;

namespace Ensek.Services.Tests;

[TestFixture]
public class MeterReadingRecordComparerTests
{
    private MeterReadingRecordComparer _comparer;

    [SetUp]
    public void Setup()
    {
        _comparer = new MeterReadingRecordComparer();
    }

    [Test]
    public void Equals_IdenticalRecords_ReturnsTrue()
    {
        // Arrange
        var dateTime = new DateTime(2024, 1, 1, 12, 0, 0);
        var record1 = new MeterReadingRecord
        {
            AccountId = 1,
            ReadingDateTime = dateTime,
            ReadingValue = 100
        };
        var record2 = new MeterReadingRecord
        {
            AccountId = 1,
            ReadingDateTime = dateTime,
            ReadingValue = 100
        };

        // Act
        var result = _comparer.Equals(record1, record2);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_DifferentAccountIds_ReturnsFalse()
    {
        // Arrange
        var dateTime = new DateTime(2024, 1, 1, 12, 0, 0);
        var record1 = new MeterReadingRecord
        {
            AccountId = 1,
            ReadingDateTime = dateTime,
            ReadingValue = 100
        };
        var record2 = new MeterReadingRecord
        {
            AccountId = 2,
            ReadingDateTime = dateTime,
            ReadingValue = 100
        };

        // Act
        var result = _comparer.Equals(record1, record2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_DifferentReadingDateTimes_ReturnsFalse()
    {
        // Arrange
        var record1 = new MeterReadingRecord
        {
            AccountId = 1,
            ReadingDateTime = new DateTime(2024, 1, 1, 12, 0, 0),
            ReadingValue = 100
        };
        var record2 = new MeterReadingRecord
        {
            AccountId = 1,
            ReadingDateTime = new DateTime(2024, 1, 1, 13, 0, 0),
            ReadingValue = 100
        };

        // Act
        var result = _comparer.Equals(record1, record2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_DifferentReadingValues_ReturnsFalse()
    {
        // Arrange
        var dateTime = new DateTime(2024, 1, 1, 12, 0, 0);
        var record1 = new MeterReadingRecord
        {
            AccountId = 1,
            ReadingDateTime = dateTime,
            ReadingValue = 100
        };
        var record2 = new MeterReadingRecord
        {
            AccountId = 1,
            ReadingDateTime = dateTime,
            ReadingValue = 200
        };

        // Act
        var result = _comparer.Equals(record1, record2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_FirstRecordNull_ReturnsFalse()
    {
        // Arrange
        MeterReadingRecord record1 = null;
        var record2 = new MeterReadingRecord
        {
            AccountId = 1,
            ReadingDateTime = new DateTime(2024, 1, 1),
            ReadingValue = 100
        };

        // Act
        var result = _comparer.Equals(record1, record2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_SecondRecordNull_ReturnsFalse()
    {
        // Arrange
        var record1 = new MeterReadingRecord
        {
            AccountId = 1,
            ReadingDateTime = new DateTime(2024, 1, 1),
            ReadingValue = 100
        };
        MeterReadingRecord record2 = null;

        // Act
        var result = _comparer.Equals(record1, record2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_BothRecordsNull_ReturnsFalse()
    {
        // Arrange
        MeterReadingRecord record1 = null;
        MeterReadingRecord record2 = null;

        // Act
        var result = _comparer.Equals(record1, record2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_IdenticalRecords_ReturnsSameHashCode()
    {
        // Arrange
        var dateTime = new DateTime(2024, 1, 1, 12, 0, 0);
        var record1 = new MeterReadingRecord
        {
            AccountId = 1,
            ReadingDateTime = dateTime,
            ReadingValue = 100
        };
        var record2 = new MeterReadingRecord
        {
            AccountId = 1,
            ReadingDateTime = dateTime,
            ReadingValue = 100
        };

        // Act
        var hashCode1 = _comparer.GetHashCode(record1);
        var hashCode2 = _comparer.GetHashCode(record2);

        // Assert
        Assert.That(hashCode1, Is.EqualTo(hashCode2));
    }

    [Test]
    public void GetHashCode_DifferentRecords_ReturnsDifferentHashCodes()
    {
        // Arrange
        var dateTime = new DateTime(2024, 1, 1, 12, 0, 0);
        var record1 = new MeterReadingRecord
        {
            AccountId = 1,
            ReadingDateTime = dateTime,
            ReadingValue = 100
        };
        var record2 = new MeterReadingRecord
        {
            AccountId = 2,
            ReadingDateTime = dateTime,
            ReadingValue = 100
        };

        // Act
        var hashCode1 = _comparer.GetHashCode(record1);
        var hashCode2 = _comparer.GetHashCode(record2);

        // Assert
        Assert.That(hashCode1, Is.Not.EqualTo(hashCode2));
    }

    [Test]
    public void GetHashCode_ConsistentResults_ReturnsSameHashCodeForSameObject()
    {
        // Arrange
        var record = new MeterReadingRecord
        {
            AccountId = 1,
            ReadingDateTime = new DateTime(2024, 1, 1, 12, 0, 0),
            ReadingValue = 100
        };

        // Act
        var hashCode1 = _comparer.GetHashCode(record);
        var hashCode2 = _comparer.GetHashCode(record);

        // Assert
        Assert.That(hashCode1, Is.EqualTo(hashCode2));
    }
}