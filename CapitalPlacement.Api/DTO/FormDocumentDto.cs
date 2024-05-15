namespace CapitalPlacement.Api.DTO
{
    public class FormDocumentDto
    {

        public string FormId { get; set; }
        public string ProgramTitle { get; set; }
        public string ProgramDescription { get; set; }
        public List<PersonalInformationDto> PersonalInformations { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }

    public class PersonalInformationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Nationality { get; set; }
        public string CurrentResidence { get; set; }
        public string IdNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
    }

    public class QuestionDto
    {
        public string QuestionId { get; set; }
        public string QuestionType { get; set; }
        public string Question { get; set; }
    }

    public class SubmitionDocumentDto
    {
        public string FormId { get; set; }
        public string ProgramTitle { get; set; }
        public string ProgramDescription { get; set; }
        public List<PersonalInformationDto> PersonalInformations { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }
    public class AnswerDto
    {
        public string QuestionId { get; set; }
        public string QuestionType { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
