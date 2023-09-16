
using System.Text.Json.Serialization;

namespace Shared.Models;

public class ProductBase
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";

    public string Name { get; set; }= "";

    public string Description { get; set; }= "";

    public string Image { get; set; } = "";
}