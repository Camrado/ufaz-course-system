namespace CourseSystem.Exceptions.Exceptions;

public class ValidationException : BaseException
{
    public List<ValidationError> ValidationErrors { get; }

    public ValidationException(List<ValidationError> validationErrors) : base("Validation errors occurred.")
    {
        ValidationErrors = validationErrors;
    }
}

public record ValidationError(string PropertyName, string ErrorMessage);
