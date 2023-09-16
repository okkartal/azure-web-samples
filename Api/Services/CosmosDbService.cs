using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using Shared.Models;

namespace Api.Services;
public class CosmosDbService : IDocumentDBService
{
    private readonly DocumentClient _docClient;

    private readonly string _dbName;
    private readonly string _collectionName;

    private readonly Uri _productCollectionUri;

    public CosmosDbService(IOptions<CosmosDbServiceOptions> options, DocumentClient client)
    {
        _dbName = options.Value.DbName;
        _collectionName = options.Value.DbCollection;

        _docClient = client;
        _productCollectionUri = UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName);
    }

    public async Task<T> AddProductAsync<T>(T product)
    {
        var dbResponse = await _docClient.CreateDocumentAsync(_productCollectionUri, product);

        return (dynamic)dbResponse.Resource;
    }

    public async Task<List<ProductBase>> GetProductsAsync()
    {
        var productsList = new List<ProductBase>();

        var products = await _docClient.ReadDocumentFeedAsync(_productCollectionUri);

        foreach (var item in products)
        {
            productsList.Add((ProductBase)item);
        }

        return productsList;
    }

    public async Task<ProductBase> GetProductAsync(string id)
    {
        var docUri = UriFactory.CreateDocumentUri(_dbName, _collectionName, id);

        Document doc = await _docClient.ReadDocumentAsync(docUri, new RequestOptions
        {
            PartitionKey = new PartitionKey(Undefined.Value)
        });

        return (doc.GetPropertyValue<string[]>("Sizes") != null)
            ? (ClothingProduct)(dynamic)doc
            : (NutritionalProduct)(dynamic)doc;
    }

    public async Task AddImageToProductAsync(string id, string imageUrl)
    {
        var docUri = UriFactory.CreateDocumentUri(_dbName, _collectionName, id);

        var doc = await _docClient.ReadDocumentAsync(docUri, new RequestOptions
        {
            PartitionKey = new PartitionKey(Undefined.Value)
        });
        
        doc.Resource.SetPropertyValue("image", imageUrl);

        await _docClient.ReplaceDocumentAsync(doc);
    }
}