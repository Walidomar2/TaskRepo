using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using LoggingSystem.API.Controllers;
using LoggingSystem.API.Dtos;
using LoggingSystem.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogginSystem.Tests.Controller
{
    public class LogsControllerTests
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly ILogStorage _logStorage;

        public LogsControllerTests()
        {
            _logRepository = A.Fake<ILogRepository>();
            _mapper = A.Fake<IMapper>();
            _logStorage = A.Fake<ILogStorage>();
        }

        [Fact]
        public async Task LogsController_GetAll_ReturnOk()
        {
            //Arrange
            var logDtos = A.Fake<ICollection<LogDto>>();
            var logDtosList = A.Fake<List<LogDto>>();
            A.CallTo(()=> _mapper.Map<List<LogDto>>(logDtos)).Returns(logDtosList);
            var controller = new LogsController(_logRepository, _mapper, _logStorage);

            //Act
            var result = await controller.GetAll(null, null,null, null);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async Task LogsController_Create_ReturnCreated()
        {
            var createdLogDto = A.Fake<CreateLogDto>();

            createdLogDto.Service = "Mock Service";
            createdLogDto.BackendType = "S3";
            createdLogDto.Level = "info";
            createdLogDto.Message = "Mock Test Message";
            createdLogDto.Timestamp = DateTime.Now;

            var controller = new LogsController(_logRepository, _mapper, _logStorage);

            var result = await controller.Create(createdLogDto);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(CreatedAtActionResult));
        }
    }
}
