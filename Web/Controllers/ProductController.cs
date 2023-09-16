using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Web.Clients;

namespace Web.Controllers;

public class ProductController : Controller
{
  private readonly ApiClient _apiClient;

  public ProductController(ApiClient apiClient)
  {
    _apiClient = apiClient;
  }
  // GET: /<controller>/
  public async Task<IActionResult> Index()
  {
    var response = await _apiClient.GetStringAsync("product");
    var products = JsonArray.Parse(response);
    return View(products.AsArray());
  }

  public async Task<IActionResult> Detail(int id)
  {
    var response = await _apiClient.GetStringAsync(
      $"product/{id}" );

    var product =  JsonObject.Parse(response);
    return View(product.AsObject());
  }
}