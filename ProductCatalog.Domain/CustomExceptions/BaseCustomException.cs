namespace ProductCatalog.Domain.CustomExceptions;

public class BaseCustomException :Exception
{
    public BaseCustomException() : base()
    {
        
    }
    
    public BaseCustomException(string message) : base(message)
    {
        
    }
}