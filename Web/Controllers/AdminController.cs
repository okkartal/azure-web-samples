using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Web.Clients; 

namespace Web.Controllers;

public class AdminController : Controller
{
    private readonly ApiClient _apiClient;

    public AdminController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    
    //Add Product
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> AddNutritional(NutritionalProduct newProduct)
    {
        //Call API to add new product
        var response = await _apiClient.PostAsJsonAsync<NutritionalProduct>("product/Nutritional", newProduct);
        
        //check response for errors
        if (response.IsSuccessStatusCode)
        {
            var model = await response.Content.ReadFromJsonAsync<NutritionalProduct>();
            ViewData["NutritionalModel"] = model;
            ViewData["NewProductId"] = model.Id;
            ViewData["ProgressMessage"] = "Product Created";
            return View("Index");
        }
        else
        {
            throw new ApplicationException(response.ReasonPhrase);
        }
    }

    public async Task<IActionResult> AddClothing(ClothingProduct newProduct)
    {
        //Call API to add new product
        var response = await _apiClient.PostAsJsonAsync<ClothingProduct>("product/Clothing", newProduct);
        
        //check response for errors
        if (response.IsSuccessStatusCode)
        {
            var model = await response.Content.ReadFromJsonAsync<ClothingProduct>();
            ViewData["ClothingModel"] = model;
            ViewData["NewProductId"] = model.Id;
            ViewData["ProgressMessage"] = "Product Created";
            return View("Index");
        }
        else
        {
            throw new ApplicationException(response.ReasonPhrase);
        }
    }

    public async Task<IActionResult> NewImage(IFormFile imageFile)
    {
        //productId from query string
        string id = Request.Form["imageProductId"];

        if (imageFile.Length > 0)
        {
            //create form payload to pass to web API 
            var imageContent = new StreamContent(imageFile.OpenReadStream());
            imageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "imageFile",
                FileName = imageFile.FileName
            };

            var postContent = new MultipartFormDataContent();
            postContent.Add(imageContent, "imageFile");
            
            //call web api passing multipart form data 
            var result = await _apiClient.PostAsJsonAsync($"product/image/{id}", postContent);

            if (result.IsSuccessStatusCode)
            {
                ViewData["ProgressMessage"] = "Image Added";
                return View("Index");
            }
            else
            {
                throw new ApplicationException(result.ReasonPhrase);
            }
        }
        else
        {
            return BadRequest();
        }
    }
}