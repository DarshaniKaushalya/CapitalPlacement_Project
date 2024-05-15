using CapitalPlacement.Api.Core;

namespace CapitalPlacement.Api.Data
{
    public interface IFormSubmitionRepository
    {

        Task<SubmitionDocument> CreateProgramFormSubmisionAsync(SubmitionDocument form);
        Task<SubmitionDocument> GetFormAnswerAsync(string formId);
    }
}
