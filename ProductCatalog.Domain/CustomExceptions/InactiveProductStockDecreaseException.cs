namespace ProductCatalog.Domain.CustomExceptions;

public class InactiveProductStockDecreaseException()
    : BaseCustomException("Can't Decrease Stock Quantity For An Inactive Product!");
