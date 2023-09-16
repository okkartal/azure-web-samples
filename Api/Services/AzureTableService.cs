using Api.Models;
using Microsoft.Azure.Cosmos.Table;

namespace Api.Services;
public class AzureTableService : ITableService
{
    private readonly string? _tableName;
    private readonly string? _partitionName;
    private readonly IConfiguration _configuration;

    public AzureTableService(IConfiguration configuration)
    {
        _configuration = configuration;
        _tableName = _configuration[Constants.KeyTable];
        _partitionName = _configuration[Constants.KeyTablePartition];
    }
    
    public async Task<List<OrderHistoryItem>> GetOrderHistoryAsync()
    {
        CloudStorageAccount csa = CloudStorageAccount.Parse(_configuration[Constants.KeyStorageCnn]);
        CloudTableClient tableClient = csa.CreateCloudTableClient();

        var table = tableClient.GetTableReference(_tableName);

        await table.CreateIfNotExistsAsync();

        var historyQuery = new TableQuery<OrderHistoryItem>()
            .Where(TableQuery.GenerateFilterCondition(
                "PartitionKey", QueryComparisons.Equal, _partitionName));

        TableContinuationToken queryToken = null;

        var tableItems = await table.ExecuteQuerySegmentedAsync<OrderHistoryItem>
            (historyQuery, queryToken);

        return tableItems.ToList();
    }
}