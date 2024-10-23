namespace Ensek.Services.Models;

public class CsvParsingResults<T>
{
    public List<string> DuplicateRecords { get; set; }
    public List<string> InvalidRecords { get; set; }
    public List<T> ValidRecords { get; set; }
    public int TotalRecords => ValidRecords.Count + InvalidRecords.Count;
    public int SavedRecords => ValidRecords.Count;
}