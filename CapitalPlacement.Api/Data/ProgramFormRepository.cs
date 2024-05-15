using CapitalPlacement.Api.Core;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CapitalPlacement.Api.Data
{
    public class ProgramFormRepository: IProgramFormRepository
    {
        private readonly CosmosClient cosmosClient;
        private readonly IConfiguration configuration;
        private readonly Container _taskContainer;

        public ProgramFormRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            this.cosmosClient = cosmosClient;
            this.configuration = configuration;
            var databaseName = configuration["CosmosDbSettings:DatabaseName"];
            var taskContainerName = "Forms";
            _taskContainer = cosmosClient.GetContainer(databaseName, taskContainerName);
        }

        public async Task<QuestionsDocument> CreateFormAsync(QuestionsDocument question)
        {
            var response = await _taskContainer.CreateItemAsync(question);
            return response.Resource;
        }

        public async Task<QuestionsDocument> GetFormByIdAsync(string Id, string questionId)
        {
            var query = _taskContainer.GetItemLinqQueryable<QuestionsDocument>()
              .Where(t => t.Id == Id && t.QuestionId == questionId)
              .Take(1)
              .ToQueryDefinition();

            var sqlQuery = query.QueryText; // Retrieve the SQL query

            var response = await _taskContainer.GetItemQueryIterator<QuestionsDocument>(query).ReadNextAsync();
            return response.FirstOrDefault();
        }
        //Full form creating ----------------------------
        public async Task<FormDocument> CreateProgramFormAsync(FormDocument form)
        {
            var response = await _taskContainer.CreateItemAsync(form);
            return response.Resource;
        }
        //Questions get by questions type
        public async Task<List<QuestionsDocument>> GetQuestionsByQuestionTypeAsync(string questiontype)
        {
            var query = _taskContainer.GetItemLinqQueryable<FormDocument>()
            .Where(f => f.Questions.Any(q => q.QuestionType == questiontype))
            .SelectMany(f => f.Questions.Where(q => q.QuestionType == questiontype))
            .ToQueryDefinition();

            var response = await _taskContainer.GetItemQueryIterator<QuestionsDocument>(query).ReadNextAsync();
            return response.ToList();
        }
        //get form by id
        public async Task<FormDocument> GetFormAsync(string formId)
        {
            var query = _taskContainer.GetItemLinqQueryable<FormDocument>()
            .Where(t => t.FormId == formId)
            .Take(1)
            .ToQueryDefinition();

            var sqlQuery = query.QueryText; // Retrieve the SQL query

            var response = await _taskContainer.GetItemQueryIterator<FormDocument>(query).ReadNextAsync();
            return response.FirstOrDefault();

        }

        public async Task<FormDocument> UpdateFormAsync(FormDocument form)
        {
            var response = await _taskContainer.ReplaceItemAsync(form, form.Id);
            return response.Resource;
        }
    }
}
