namespace Shared.Exceptions;

public class BaseCustomException :Exception
{
    public BaseCustomException() : base()
    {
        
    }
    
    public BaseCustomException(string message) : base(message)
    {
        
    }
}