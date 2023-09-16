using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public  class OrderItem
{
    public string Id { get; set; }

    public string Name { get; set; }

    public int Quantity { get; set; }

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Size { get; set; }
}