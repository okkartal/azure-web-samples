namespace Web.Clients;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T data)
    {
        return await _httpClient.PostAsJsonAsync<T>(requestUri, data);
    }

    public async Task<string> GetStringAsync(string requestUri)
    {
        return await _httpClient.GetStringAsync(requestUri);
    }
}