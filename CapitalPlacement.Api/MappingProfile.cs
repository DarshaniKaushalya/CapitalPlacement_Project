using AutoMapper;
using CapitalPlacement.Api.Core;
using CapitalPlacement.Api.DTO;

namespace CapitalPlacement.Api
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            //Mapping for get questions by questiontype
            CreateMap<QuestionDto, QuestionsDocument>();
            CreateMap<QuestionsDocument, QuestionDto>();

            // Mapping for creating form documents
            CreateMap<FormDocumentDto, FormDocument>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignore Id because it's set manually
            CreateMap<PersonalInformationDto, personalInformationsDocument>();
            CreateMap<QuestionDto, QuestionsDocument>();

            CreateMap<FormDocument, FormDocumentDto>();
            CreateMap<personalInformationsDocument, PersonalInformationDto>();
            CreateMap<QuestionsDocument, QuestionDto>();

            // Mapping for submission form answer documents
            CreateMap<SubmitionDocumentDto, SubmitionDocument>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<PersonalInformationDto, personalInformationsDocument>();
            CreateMap<AnswerDto, AnswerDocument>();

            CreateMap<SubmitionDocument, SubmitionDocumentDto>();
            CreateMap<personalInformationsDocument, PersonalInformationDto>();
            CreateMap<AnswerDocument, AnswerDto>();
        }
    }
}
