using Web.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("MyClient", config =>
{
    config.BaseAddress = new Uri(builder.Configuration.GetSection("ApiBaseUrl").Value);
});

builder.Services.AddSingleton<ApiClient>(c =>
{
    var factory = c.GetService<IHttpClientFactory>();
    var httpClient = factory.CreateClient("MyClient");
    httpClient.DefaultRequestHeaders.Clear();
    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
    httpClient.Timeout = TimeSpan.FromMinutes(1);
    return new ApiClient(httpClient);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();