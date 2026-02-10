namespace ProductCatalog.Domain.CustomExceptions;

public class NegativeStockQuantityException() :BaseCustomException("Requested Amount Of Stock Must Be Positive To Decrease!");