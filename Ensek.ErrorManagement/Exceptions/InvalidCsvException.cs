namespace Ensek.ErrorManagement.Exceptions;

public class InvalidCsvException : Exception
{
    public InvalidCsvException()
    {
    }

    public InvalidCsvException(string message)
        : base(message)
    {
    }

    public InvalidCsvException(string message, Exception inner)
        : base(message, inner)
    {
    }
}