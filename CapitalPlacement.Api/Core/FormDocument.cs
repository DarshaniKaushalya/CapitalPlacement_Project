using Newtonsoft.Json;

namespace CapitalPlacement.Api.Core
{
    public class FormDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("formId")]
        public string FormId { get; set; }

        [JsonProperty("programTitle")]
        public string ProgramTitle { get; set; }

        [JsonProperty("programDescription")]
        public string ProgramDescription { get; set; }

        [JsonProperty("personalInformations")]
        public List<personalInformationsDocument> PersonalInformations { get; set; }

        [JsonProperty("questions")]
        public List<QuestionsDocument> Questions { get; set; }
    }
    public class QuestionsDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("questionId")]
        public string QuestionId { get; set; }

        [JsonProperty("questiontype")]
        public string QuestionType { get; set; }

        [JsonProperty("question")]
        public string Question { get; set; }
    }
    public class personalInformationsDocument
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("currentResidence")]
        public string CurrentResidence { get; set; }

        [JsonProperty("idNumber")]
        public string IdNumber { get; set; }

        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }
    }

    public class SubmitionDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("formId")]
        public string FormId { get; set; }

        [JsonProperty("programTitle")]
        public string ProgramTitle { get; set; }

        [JsonProperty("programDescription")]
        public string ProgramDescription { get; set; }

        [JsonProperty("personalInformations")]
        public List<personalInformationsDocument> PersonalInformations { get; set; }

        [JsonProperty("answers")]
        public List<AnswerDocument> Answers { get; set; }
    }
    public class AnswerDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("questionId")]
        public string QuestionId { get; set; }

        [JsonProperty("questiontype")]
        public string QuestionType { get; set; }

        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }
    }
}
