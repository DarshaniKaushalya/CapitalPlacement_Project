using AutoMapper;
using CapitalPlacement.Api.Core;
using CapitalPlacement.Api.Data;
using CapitalPlacement.Api.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapitalPlacement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramFormController : ControllerBase
    {
        private readonly IProgramFormRepository _programFormRepository;
        private readonly IFormSubmitionRepository _formSubmitionRepository;
        IMapper _mapper;

        public ProgramFormController(IProgramFormRepository programFormRepository, IFormSubmitionRepository formSubmitionRepository, IMapper mapper)
        {
            _programFormRepository = programFormRepository;
            _formSubmitionRepository = formSubmitionRepository;
            _mapper = mapper;
        }

        //Create form
        [HttpPost("CreateForm")]
        public async Task<ActionResult<FormDocumentDto>> CreateProgramForm(FormDocumentDto formDto)
        {
            try
            {
                if (formDto == null)
                {
                    return BadRequest("Form cannot be null.");
                }

                var form = _mapper.Map<FormDocument>(formDto);
                form.Id = Guid.NewGuid().ToString();

                var createdForm = await _programFormRepository.CreateProgramFormAsync(form);
                var createdFormDto = _mapper.Map<FormDocumentDto>(createdForm);

                return CreatedAtAction(nameof(GetForm), new { formId = createdFormDto.FormId }, createdFormDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the form: {ex.Message}");
            }
        }
        //Get Form By formId
        [HttpGet("{formId}")]
        public async Task<ActionResult<FormDocumentDto>> GetForm(string formId)
        {
            var form = await _programFormRepository.GetFormAsync(formId);
            if (form == null)
            {
                return NotFound();
            }
            var formDto = _mapper.Map<FormDocumentDto>(form);
            return Ok(formDto);
        }

        //Get questions by questiontype
        [HttpGet("Questions/ByQuestionType/{questiontype}")]
        public async Task<ActionResult<List<QuestionDto>>> GetQuestionsByQuestiontype(string questiontype)
        {
            var questions = await _programFormRepository.GetQuestionsByQuestionTypeAsync(questiontype);
            if (questions == null || !questions.Any())
            {
                return NotFound();
            }

            var questionsDto = _mapper.Map<List<QuestionDto>>(questions);
            return Ok(questionsDto);
        }

        //Update Questions search by {formId} and  {questionId}
        [HttpPut("UpdateQuestion/{formId}/{questionId}")]
        public async Task<ActionResult<FormDocumentDto>> UpdateQuestionInForm(string formId, string questionId, QuestionDto updatedQuestionDto)
        {
            var form = await _programFormRepository.GetFormAsync(formId);
            if (form == null)
            {
                return NotFound();
            }

            var question = form.Questions.FirstOrDefault(q => q.QuestionId == questionId);
            if (question == null)
            {
                return NotFound($"Question with ID {questionId} not found in form {formId}.");
            }
            _mapper.Map(updatedQuestionDto, question);

            try
            {
                var updatedForm = await _programFormRepository.UpdateFormAsync(form);
                var updatedFormDto = _mapper.Map<FormDocumentDto>(updatedForm);

                return Ok(updatedFormDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the question in the form: {ex.Message}");
            }
        }

        //Form Submission
        [HttpPost("SubmitForm")]
        public async Task<ActionResult<SubmitionDocumentDto>> CreateProgramFormOne(SubmitionDocumentDto formDto)
        {
            try
            {
                var form = _mapper.Map<SubmitionDocument>(formDto);
                form.Id = Guid.NewGuid().ToString();

                var createdSubmision = await _formSubmitionRepository.CreateProgramFormSubmisionAsync(form);
                var createdSubmisionDto = _mapper.Map<SubmitionDocumentDto>(createdSubmision);

                return CreatedAtAction(nameof(GetFormAnswer), new { formId = createdSubmision.FormId }, createdSubmisionDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the form: {ex.Message}");
            }
        }

        //Get Submited Form Answers by formId
        [HttpGet("FormAnswer/{formId}")]
        public async Task<ActionResult<SubmitionDocumentDto>> GetFormAnswer(string formId)
        {
            var form = await _formSubmitionRepository.GetFormAnswerAsync(formId);
            if (form == null)
            {
                return NotFound();
            }

            var formDto = _mapper.Map<SubmitionDocumentDto>(form);
            return Ok(formDto);
        }

    }
}
