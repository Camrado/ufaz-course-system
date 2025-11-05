namespace CourseSystem.Exceptions.Exceptions;

public class BaseException : Exception
{
    public object[] Parameters { get; }
    public string ExceptionMessage { get; }

    public BaseException(string message) : base(message)
    {
        ExceptionMessage = message;
    }

    public BaseException(string message, params object[] parameters) : base(message)
    {
        Parameters = parameters;
        ExceptionMessage = message;
    }
}
