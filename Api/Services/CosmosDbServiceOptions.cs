namespace Api.Services;

public class CosmosDbServiceOptions
{
    public string DbUri { get; set; }

    public string DbKey { get; set; }

    public string DbName { get; set; }

    public string DbCollection { get; set; }
}