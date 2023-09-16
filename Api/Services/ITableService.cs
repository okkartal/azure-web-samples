using Api.Models;

namespace Api.Services;

public interface ITableService
{
    Task<List<OrderHistoryItem>> GetOrderHistoryAsync();
}