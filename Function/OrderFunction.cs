using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Shared.Models;

namespace Function;

[Table("orderhistory")]
public static class OrderFunction
{
    [FunctionName("OrderFunction")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", 
            Route = null)] HttpRequestMessage req,
        ICollector<TableOrderItem> items,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");
        string orderId = System.Guid.NewGuid().ToString();

        Order order = await req.Content.ReadAsAsync<Order>();

        foreach (var item in order.Items)
        {
            TableOrderItem toi = new TableOrderItem(item);
            toi.PartitionKey = "history";
            toi.RowKey = $"{orderId} - {item.Id}";
            items.Add(toi);
        }

        return new OkResult();
    }
}