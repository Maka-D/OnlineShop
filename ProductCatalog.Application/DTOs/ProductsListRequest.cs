namespace ProductCatalog.Application.DTOs;

public class ProductsListRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}