namespace ProductCatalog.Application.DTOs;

public class ProductDetailsRequest
{
    public int ProductId { get; set; }
}

public class ProductDetailsChunkRequest
{
    public IEnumerable<int> Ids { get; set; }
}