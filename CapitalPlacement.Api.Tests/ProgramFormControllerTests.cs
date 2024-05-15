using AutoMapper;
using CapitalPlacement.Api.Controllers;
using CapitalPlacement.Api.Core;
using CapitalPlacement.Api.Data;
using CapitalPlacement.Api.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CapitalPlacement.Api.Tests
{
    public class ProgramFormControllerTests
    {
        private readonly Mock<IProgramFormRepository> _mockRepository;
        private readonly Mock<IFormSubmitionRepository> _mockSubmitionRepository;
        private readonly IMapper _mapper;
        private readonly ProgramFormController _controller;

        public ProgramFormControllerTests()
        {
            _mockRepository = new Mock<IProgramFormRepository>();
            _mockSubmitionRepository = new Mock<IFormSubmitionRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FormDocument, FormDocumentDto>().ReverseMap();
                cfg.CreateMap<QuestionsDocument, QuestionDto>().ReverseMap();
                cfg.CreateMap<personalInformationsDocument, PersonalInformationDto>().ReverseMap();
                cfg.CreateMap<SubmitionDocument, SubmitionDocumentDto>().ReverseMap();
                cfg.CreateMap<AnswerDocument, AnswerDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            _controller = new ProgramFormController(_mockRepository.Object, _mockSubmitionRepository.Object, _mapper);
        }

        //Form Creation tests
        [Fact]
        public async Task CreateProgramForm_ReturnsCreatedAtActionResult_WhenFormIsValid()
        {
            var formDto = new FormDocumentDto
            {
                ProgramTitle = "Test Program",
                ProgramDescription = "Test Description"
            };
            var form = _mapper.Map<FormDocument>(formDto);
            form.Id = Guid.NewGuid().ToString();

            _mockRepository.Setup(repo => repo.CreateProgramFormAsync(It.IsAny<FormDocument>()))
                           .ReturnsAsync(form);

            var result = await _controller.CreateProgramForm(formDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<FormDocumentDto>(createdAtActionResult.Value);
            Assert.Equal(formDto.ProgramTitle, returnValue.ProgramTitle);
            Assert.Equal(formDto.ProgramDescription, returnValue.ProgramDescription);
        }

        [Fact]
        public async Task CreateProgramForm_ReturnsBadRequest_WhenFormDtoIsNull()
        {
            var result = await _controller.CreateProgramForm(null);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateProgramForm_ReturnsStatusCode500_WhenExceptionIsThrown()
        {
            var formDto = new FormDocumentDto
            {
                ProgramTitle = "Test Program",
                ProgramDescription = "Test Description"
            };

            _mockRepository.Setup(repo => repo.CreateProgramFormAsync(It.IsAny<FormDocument>()))
                           .ThrowsAsync(new Exception("Test Exception"));

            var result = await _controller.CreateProgramForm(formDto);

            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("An error occurred while creating the form: Test Exception", objectResult.Value);
        }

        [Fact]
        public async Task GetForm_ReturnsOkObjectResult_WhenFormExists()
        {
            var formId = "test-form-id";
            var form = new FormDocument
            {
                Id = formId,
                FormId = formId,
                ProgramTitle = "Test Program",
                ProgramDescription = "Test Description"
            };

            _mockRepository.Setup(repo => repo.GetFormAsync(formId))
                           .ReturnsAsync(form);

            var result = await _controller.GetForm(formId);

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<FormDocumentDto>(okObjectResult.Value);
            Assert.Equal(formId, returnValue.FormId);
            Assert.Equal("Test Program", returnValue.ProgramTitle);
            Assert.Equal("Test Description", returnValue.ProgramDescription);
        }

        [Fact]
        public async Task GetForm_ReturnsNotFoundResult_WhenFormDoesNotExist()
        {
            var formId = "non-existent-form-id";

            _mockRepository.Setup(repo => repo.GetFormAsync(formId))
                           .ReturnsAsync((FormDocument)null);

            var result = await _controller.GetForm(formId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        //Get Question By QuestionType tests
        [Fact]
        public async Task GetQuestionsByQuestiontype_ReturnsOkObjectResult_WhenQuestionsExist()
        {
            var questionType = "test-type";
            var questions = new List<QuestionsDocument>
            {
                new QuestionsDocument { QuestionId = "1", Question = "Question 1", QuestionType = questionType },
                new QuestionsDocument { QuestionId = "2", Question = "Question 2", QuestionType = questionType }
            };

            _mockRepository.Setup(repo => repo.GetQuestionsByQuestionTypeAsync(questionType))
                           .ReturnsAsync(questions);

            var result = await _controller.GetQuestionsByQuestiontype(questionType);

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<QuestionDto>>(okObjectResult.Value);
            Assert.Equal(2, returnValue.Count);
            Assert.Equal("Question 1", returnValue[0].Question);
            Assert.Equal("Question 2", returnValue[1].Question);
        }

        [Fact]
        public async Task GetQuestionsByQuestiontype_ReturnsNotFoundResult_WhenNoQuestionsExist()
        {
            var questionType = "non-existent-type";

            _mockRepository.Setup(repo => repo.GetQuestionsByQuestionTypeAsync(questionType))
                           .ReturnsAsync((List<QuestionsDocument>)null);

            var result = await _controller.GetQuestionsByQuestiontype(questionType);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetQuestionsByQuestiontype_ReturnsNotFoundResult_WhenQuestionsListIsEmpty()
        {
            var questionType = "empty-type";
            var emptyQuestionsList = new List<QuestionsDocument>();

            _mockRepository.Setup(repo => repo.GetQuestionsByQuestionTypeAsync(questionType))
                           .ReturnsAsync(emptyQuestionsList);

            var result = await _controller.GetQuestionsByQuestiontype(questionType);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        //Update - unit tests
        [Fact]
        public async Task UpdateQuestionInForm_ReturnsOkObjectResult_WhenUpdateIsSuccessful()
        {
            var formId = "test-form-id";
            var questionId = "test-question-id";
            var updatedQuestionDto = new QuestionDto { QuestionId = questionId, Question = "Updated Question" };
            var form = new FormDocument
            {
                Id = formId,
                FormId = formId,
                Questions = new List<QuestionsDocument>
                {
                    new QuestionsDocument { QuestionId = questionId, Question = "Old Question" }
                }
            };

            _mockRepository.Setup(repo => repo.GetFormAsync(formId)).ReturnsAsync(form);
            _mockRepository.Setup(repo => repo.UpdateFormAsync(It.IsAny<FormDocument>())).ReturnsAsync(form);

            var result = await _controller.UpdateQuestionInForm(formId, questionId, updatedQuestionDto);

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<FormDocumentDto>(okObjectResult.Value);
            Assert.Equal(formId, returnValue.FormId);
            Assert.Equal(updatedQuestionDto.Question, returnValue.Questions.First(q => q.QuestionId == questionId).Question);
        }

        [Fact]
        public async Task UpdateQuestionInForm_ReturnsNotFoundResult_WhenFormDoesNotExist()
        {
            var formId = "non-existent-form-id";
            var questionId = "test-question-id";
            var updatedQuestionDto = new QuestionDto { QuestionId = questionId, Question = "Updated Question" };

            _mockRepository.Setup(repo => repo.GetFormAsync(formId)).ReturnsAsync((FormDocument)null);

            var result = await _controller.UpdateQuestionInForm(formId, questionId, updatedQuestionDto);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateQuestionInForm_ReturnsNotFoundResult_WhenQuestionDoesNotExistInForm()
        {
            var formId = "test-form-id";
            var questionId = "non-existent-question-id";
            var updatedQuestionDto = new QuestionDto { QuestionId = questionId, Question = "Updated Question" };
            var form = new FormDocument
            {
                Id = formId,
                FormId = formId,
                Questions = new List<QuestionsDocument>
                {
                    new QuestionsDocument { QuestionId = "existing-question-id", Question = "Existing Question" }
                }
            };

            _mockRepository.Setup(repo => repo.GetFormAsync(formId)).ReturnsAsync(form);

            var result = await _controller.UpdateQuestionInForm(formId, questionId, updatedQuestionDto);

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Question with ID {questionId} not found in form {formId}.", notFoundObjectResult.Value);
        }

        [Fact]
        public async Task UpdateQuestionInForm_ReturnsStatusCode500_WhenExceptionIsThrown()
        {
            var formId = "test-form-id";
            var questionId = "test-question-id";
            var updatedQuestionDto = new QuestionDto { QuestionId = questionId, Question = "Updated Question" };
            var form = new FormDocument
            {
                Id = formId,
                FormId = formId,
                Questions = new List<QuestionsDocument>
                {
                    new QuestionsDocument { QuestionId = questionId, Question = "Old Question" }
                }
            };

            _mockRepository.Setup(repo => repo.GetFormAsync(formId)).ReturnsAsync(form);
            _mockRepository.Setup(repo => repo.UpdateFormAsync(It.IsAny<FormDocument>())).ThrowsAsync(new Exception("Test Exception"));

            var result = await _controller.UpdateQuestionInForm(formId, questionId, updatedQuestionDto);

            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("An error occurred while updating the question in the form: Test Exception", objectResult.Value);
        }

        //Form Submision - Unit tests
        [Fact]
        public async Task CreateProgramFormOne_ReturnsCreatedAtActionResult_WhenFormIsValid()
        {
            var formDto = new SubmitionDocumentDto
            {
                FormId = "test-form-id",
                ProgramTitle = "test-programTitle",
                Answers = new List<AnswerDto> { new AnswerDto { QuestionId = "q1", Answer = "Answer 1" } }
            };
            var form = _mapper.Map<SubmitionDocument>(formDto);
            form.Id = Guid.NewGuid().ToString();

            _mockSubmitionRepository.Setup(repo => repo.CreateProgramFormSubmisionAsync(It.IsAny<SubmitionDocument>()))
                                    .ReturnsAsync(form);

            var result = await _controller.CreateProgramFormOne(formDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<SubmitionDocumentDto>(createdAtActionResult.Value);
            Assert.Equal(formDto.FormId, returnValue.FormId);
            Assert.Equal(formDto.ProgramTitle, returnValue.ProgramTitle);
        }

        [Fact]
        public async Task CreateProgramFormOne_ReturnsStatusCode500_WhenExceptionIsThrown()
        {
            var formDto = new SubmitionDocumentDto
            {
                FormId = "test-form-id",
                ProgramTitle = "test-programTitle",
                Answers = new List<AnswerDto> { new AnswerDto { QuestionId = "q1", Answer = "Answer 1" } }
            };

            _mockSubmitionRepository.Setup(repo => repo.CreateProgramFormSubmisionAsync(It.IsAny<SubmitionDocument>()))
                                    .ThrowsAsync(new Exception("Test Exception"));

            var result = await _controller.CreateProgramFormOne(formDto);

            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("An error occurred while creating the form: Test Exception", objectResult.Value);
        }
    }
}

