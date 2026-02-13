using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain.Interfaces;
using Shared.Models;

namespace ProductCatalog.Application.Services;

public class ProductCatalogService :IProductCatalogService
{
    private readonly IProductCatalogRepository _productCatalogRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductCatalogService(IProductCatalogRepository productCatalogRepository, IUnitOfWork unitOfWork)
    {
        _productCatalogRepository = productCatalogRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<ProductsListResponse>> GetProductsAsync(ProductsListRequest request)
    {
        var products = await _productCatalogRepository.GetProductsAsync(request.PageSize, request.PageNumber);
        var productDtos = products.Select(p => new Product()
        {
            Id =  p.Id,
            Name = p.Name,
            Price = p.Price,
            SKU = p.SKU,
            StockQuantity = p.StockQuantity,
            IsActive = p.IsActive 
        });
        return  Result<ProductsListResponse>.Success(new ProductsListResponse()
        {
            Products = productDtos
        });
    }

    public async Task<Result<ProductDetailsResponse>> GetProductDetailsAsync(ProductDetailsRequest request)
    {
        var product = await _productCatalogRepository.GetProductDetailsAsync(request.ProductId);
        if (product == null)
            return Result<ProductDetailsResponse>.Failure("Product not found");
        return Result<ProductDetailsResponse>.Success(new ProductDetailsResponse()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            SKU =  product.SKU,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive
        });
    }

    public async Task<Result<ProductDetailsChunkResponse>> GetProductDetailsByIdsAsync(ProductDetailsChunkRequest request)
    {
       var requestedIds = request.Ids.Distinct().ToList();
       var existingProducts = await _productCatalogRepository.GetProductsByIdsAsync(request.Ids);

       if (existingProducts.Count() != requestedIds.Count)
       {
           var foundIds = existingProducts.Select(p => p.Id);
           var missingIds = requestedIds.Except(foundIds);
    
           return Result<ProductDetailsChunkResponse>.Failure(
               $"The following products do not exist: {string.Join(", ", missingIds)}");
       }

       var resultData = existingProducts.Select(p => new ProductDetailsResponse()
       {
           Id = p.Id,
           Name = p.Name,
           Price = p.Price,
           SKU =  p.SKU,
           StockQuantity = p.StockQuantity,
           IsActive = p.IsActive
       });
       return Result<ProductDetailsChunkResponse>.Success(new ProductDetailsChunkResponse(){ Products =  resultData });
    }

    public async Task<Result<ProductCreateResponse>> CreateProductAsync(Product product)
    {
        var skuCheck = await _productCatalogRepository.ExistsBySKU(product.SKU);
        if(skuCheck)
            return Result<ProductCreateResponse>.Failure("Product with same SKU already exists");

        var domainProduct = new Domain.Models.Product()
        {
            Name = product.Name,
            Price = product.Price,
            SKU = product.SKU,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive
        };
        _productCatalogRepository.Create(domainProduct);
        await _unitOfWork.SaveChangesAsync();
        
        return Result<ProductCreateResponse>.Success(new ProductCreateResponse()
        {
            ProductId = domainProduct.Id
        });
    }

    public async Task<Result<bool?>> UpdateProductAsync(ProductUpdateRequest product)
    {
        var dbProduct = await _productCatalogRepository.GetProductDetailsAsync(product.Id);
        if(dbProduct == null)
            return Result<bool?>.Failure("Product not found to update");
        
        if (dbProduct.SKU != product.SKU)
        {
            var existsSku = await _productCatalogRepository.ExistsBySKU(product.SKU);
            if(existsSku)
                return Result<bool?>.Failure("Product with same SKU already exists");
        }
        
        dbProduct.SKU = product.SKU;
        dbProduct.StockQuantity =  product.StockQuantity;
        dbProduct.IsActive = product.IsActive;
        dbProduct.Price = product.Price;
        _productCatalogRepository.Update(dbProduct);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool?>.Success();
    }

    public async Task<Result<bool>> DecreaseProductStockAsync(IEnumerable<ProductStockUpdate> items)
    {
        if (items == null || !items.Any())
            return Result<bool>.Failure("No items provided.");
        
        var productIds = items.Select(i => i.ProductId).Distinct();
        var products = await _productCatalogRepository.GetProductsByIdsAsync(productIds);
        var productDict = products.ToDictionary(p => p.Id, p => p);

        await _unitOfWork.BeginTransactionAsync();
        try 
        {
            foreach (var item in items)
            {
                if (!productDict.TryGetValue(item.ProductId, out var product))
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result<bool>.Failure($"Product {item.ProductId} not found in catalog.");
                }

                product.DecreaseStockQuantity(item.Quantity);
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        
            return Result<bool>.Success();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result<bool>.Failure("Concurrency conflict: Stock was updated by another user");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result<bool>.Failure("An unexpected error occurred while updating stock.");
        }
    }
    
    public async Task<Result<bool>> AdjustStockAsync(IEnumerable<ProductStockUpdate> items)
    {
        if (items == null || !items.Any())
            return Result<bool>.Failure("No items provided.");
        
        var productIds = items.Select(i => i.ProductId).Distinct();
        var products = await _productCatalogRepository.GetProductsByIdsAsync(productIds);
        var productDict = products.ToDictionary(p => p.Id, p => p);

        await _unitOfWork.BeginTransactionAsync();
        try 
        {
            foreach (var item in items)
            {
                if (!productDict.TryGetValue(item.ProductId, out var product))
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result<bool>.Failure($"Product {item.ProductId} not found in catalog.");
                }

                product.IncreaseStockQuantity(item.Quantity);
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        
            return Result<bool>.Success();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result<bool>.Failure("An unexpected error occurred while updating stock.");
        }
    }
}