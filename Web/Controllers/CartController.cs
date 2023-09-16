using System.Net;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Web.Clients;
using Shared.Models;

namespace Web.Controllers;

public class CartController : Controller
{
    private readonly ApiClient _apiClient;

    public CartController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(Order order)
    {
        var response = await _apiClient.PostAsJsonAsync<Order>("order", order);

        if (response.StatusCode == HttpStatusCode.OK)
            return Ok();
        else
            throw new ApplicationException($"Order failed with status code : {Response.StatusCode}");
    }

    [HttpGet]
    public async Task<IActionResult> History()
    {
        var orderHistory = await _apiClient.GetStringAsync("order");
        var historyArray = JsonArray.Parse(orderHistory);
        return View(historyArray);
    }

    public IActionResult OrderComplete()
    {
        return View();
    }
}