using CapitalPlacement.Api.Core;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CapitalPlacement.Api.Data
{
    public class FormSubmitionRepository : IFormSubmitionRepository
    {
        private readonly CosmosClient cosmosClient;
        private readonly IConfiguration configuration;
        private readonly Container _taskContainer;

        public FormSubmitionRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            this.cosmosClient = cosmosClient;
            this.configuration = configuration;
            var databaseName = configuration["CosmosDbSettings:DatabaseName"];
            var FormSubmitionContainerName = "FormSubmitions";
            _taskContainer = cosmosClient.GetContainer(databaseName, FormSubmitionContainerName);
        }

        public async Task<SubmitionDocument> CreateProgramFormSubmisionAsync(SubmitionDocument form)
        {
            var response = await _taskContainer.CreateItemAsync(form);
            return response.Resource;
        }

        public async Task<SubmitionDocument> GetFormAnswerAsync(string formId)
        {
            var query = _taskContainer.GetItemLinqQueryable<SubmitionDocument>()
           .Where(t => t.FormId == formId)
           .Take(1)
           .ToQueryDefinition();

            var sqlQuery = query.QueryText; 

            var response = await _taskContainer.GetItemQueryIterator<SubmitionDocument>(query).ReadNextAsync();
            return response.FirstOrDefault();
        }
    }
}
