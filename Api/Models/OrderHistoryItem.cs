using Microsoft.Azure.Cosmos.Table;

namespace Api.Models;

public class OrderHistoryItem : TableEntity
{
    public string Id { get; set; }

    public string Name { get; set; }

    public int Quantity { get; set; }

    public string Size { get; set; }
}