using Shared.Exceptions;

namespace Orders.Domain.CustomExceptions;


public class NegativeException :BaseCustomException
{
    public NegativeException(): base("Value Cannot be negative")
    {
        
    }
    public NegativeException(string value) : base(value)
    {
        
    }
}