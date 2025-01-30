using FluentValidation.Results;

namespace Domain.Exceptions;

public class ValidationEntityException : BaseException
{
    public ValidationEntityException(List<string> messages) : base(messages)
    {
    }

    public ValidationEntityException(IEnumerable<ValidationFailure> validationFailures) : base(validationFailures)
    {
        
    }
}