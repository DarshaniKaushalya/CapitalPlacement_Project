using CapitalPlacement.Api.Core;

namespace CapitalPlacement.Api.Data
{
    public interface IProgramFormRepository
    {
        Task<FormDocument> CreateProgramFormAsync(FormDocument form);
        Task<List<QuestionsDocument>> GetQuestionsByQuestionTypeAsync(string questiontype);
        Task<FormDocument> GetFormAsync(string formId);
        Task<FormDocument> UpdateFormAsync(FormDocument form);

    }
}
