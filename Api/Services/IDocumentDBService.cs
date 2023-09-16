using Shared.Models;

namespace Api.Services;

public interface IDocumentDBService
{
    Task<T> AddProductAsync<T>(T product);

    Task<List<ProductBase>> GetProductsAsync();

    Task<ProductBase> GetProductAsync(string id);

    Task AddImageToProductAsync(string id, string imageUrl);
}