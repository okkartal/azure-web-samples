using Api.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("MyCorsPolicy")]
public class ProductController : ControllerBase
{
    private readonly IDocumentDBService _documentDbService;
    private readonly IBlobService _blobService;

    public ProductController(IDocumentDBService documentDbService, IBlobService blobService)
    {
        _documentDbService = documentDbService;
        _blobService = blobService;
    }
    
    //Get api/product
    [HttpGet]
    public async Task<JsonResult> Get()
    {
        List<ProductBase> products = await _documentDbService.GetProductsAsync();
        return new JsonResult(products);
    }

    //GET api/product/1
    [HttpGet("{id}")]
    public async Task<JsonResult> Get(string id)
    {
        var product = await _documentDbService.GetProductAsync(id);
        return new JsonResult(product);
    }

    [HttpPost]
    [Route("/api/[controller]/Nutritional")]
    public async Task<JsonResult> AddNutritional(NutritionalProduct product)
    {
        var newProduct = await _documentDbService.AddProductAsync<NutritionalProduct>(product);
        return new JsonResult(newProduct);
    }

    [HttpPost]
    [Route("/api/[controller]/Clothing")]
    public async Task<JsonResult> AddClothing(ClothingProduct product)
    {
        var newProduct = await _documentDbService.AddProductAsync<ClothingProduct>(product);
        return new JsonResult(newProduct);
    }

    [HttpPost]
    [Route("/api/[controller]/Image/{id}")]
    public async Task<IActionResult> AddImage(IFormFile imageFile)
    {
        string id = (string)RouteData.Values["id"];

        if (!Request.HasFormContentType)
            return new UnsupportedMediaTypeResult();

        var fileName = $"{id}.jpg";
        
        //BLOB Service: Get blob to write
        Stream imageStream = null;

        try
        {
            imageStream = imageFile.OpenReadStream();

            var blobRef = await _blobService.UploadBlobAsync(fileName, imageStream);

            //Update cosmos db with image link
            await _documentDbService.AddImageToProductAsync(id, blobRef);
        }
        catch  
        {
            throw;
        }
        finally
        {
            if(imageStream != null)
                imageStream.Dispose();
        }

        return Ok();
    } 
}