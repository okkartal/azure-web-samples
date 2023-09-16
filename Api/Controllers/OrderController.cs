using Api.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("MyCorsPolicy")]
public class OrderController : Controller
{
   private readonly IQueueService _queueService;
   private readonly ITableService _tableService;

   public OrderController(IQueueService queueService, ITableService tableService)
   {
      _queueService = queueService;
      _tableService = tableService;
   }

   [HttpPost]
   public async Task<IActionResult> CreateOrder(Order order)
   {
      await _queueService.SendMessageAsync(order);

      return Ok();
   }

   [HttpGet]
   public async Task<IActionResult> GetOrderHistory()
   {
      var items = await _tableService.GetOrderHistoryAsync();
      return new JsonResult(items);
   }
}