namespace CourseSystem.Exceptions.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, params object[] parameters) : base(message, parameters)
    {
    }
}
