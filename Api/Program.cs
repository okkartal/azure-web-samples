using Api;
using Api.Services;
using Microsoft.Azure.Documents.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();

//Adding custom services for DI
//Cosmos DB servuce initialized with config
IConfiguration dbConfig = builder.Configuration.GetSection(Constants.KeyDbConfig);
builder.Services.Configure<CosmosDbServiceOptions>(dbConfig);

//single doc client for performance
var docClient = new DocumentClient(new Uri(builder.Configuration[Constants.KeyCosmosUri]),
    builder.Configuration[Constants.KeyCosmosKey]);
builder.Services.AddSingleton<DocumentClient>(docClient);

//Add storage service implementations
builder.Services.AddScoped<IQueueService, AzureQueueService>();
builder.Services.AddScoped<ITableService, AzureTableService>();
builder.Services.AddScoped<IDocumentDBService, CosmosDbService>();
builder.Services.AddScoped<IBlobService, AzureBlobService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
{
    o.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5296")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("MyCorsPolicy");
app.Run();