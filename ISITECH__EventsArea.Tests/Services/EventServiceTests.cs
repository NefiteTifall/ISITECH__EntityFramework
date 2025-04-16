using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ISITECH__EventsArea.API.DTO;
using ISITECH__EventsArea.Application.Services;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using Moq;
using Xunit;

namespace ISITECH__EventsArea.Tests.Services
{
    public class EventServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(uow => uow.Events).Returns(_mockEventRepository.Object);
            _eventService = new EventService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetEventByIdAsync_ExistingId_ReturnsEventDto()
        {
            // Arrange
            var eventId = 1;
            var eventEntity = new Event
            {
                Id = eventId,
                Title = "Test Event",
                Description = "Test Description",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2),
                Status = EventStatus.Scheduled
            };

            var expectedDto = new EventDto
            {
                Id = eventId,
                Title = "Test Event",
                Description = "Test Description",
                StartDate = eventEntity.StartDate,
                EndDate = eventEntity.EndDate,
                Status = EventStatus.Scheduled
            };

            _mockEventRepository.Setup(repo => repo.GetEventWithDetailsAsync(eventId))
                .ReturnsAsync(eventEntity);
            _mockMapper.Setup(mapper => mapper.Map<EventDto>(eventEntity))
                .Returns(expectedDto);

            // Act
            var result = await _eventService.GetEventByIdAsync(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Title, result.Title);
            Assert.Equal(expectedDto.Description, result.Description);
            Assert.Equal(expectedDto.StartDate, result.StartDate);
            Assert.Equal(expectedDto.EndDate, result.EndDate);
            Assert.Equal(expectedDto.Status, result.Status);
        }

        [Fact]
        public async Task CreateEventAsync_ValidEvent_ReturnsCreatedEventDto()
        {
            // Arrange
            var createDto = new EventCreateDto
            {
                Title = "New Event",
                Description = "New Description",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2),
                Status = EventStatus.Scheduled,
                CategoryId = 1,
                LocationId = 1
            };

            var eventEntity = new Event
            {
                Id = 1,
                Title = createDto.Title,
                Description = createDto.Description,
                StartDate = createDto.StartDate,
                EndDate = createDto.EndDate,
                Status = createDto.Status,
                CategoryId = createDto.CategoryId,
                LocationId = createDto.LocationId
            };

            var expectedDto = new EventDto
            {
                Id = 1,
                Title = createDto.Title,
                Description = createDto.Description,
                StartDate = createDto.StartDate,
                EndDate = createDto.EndDate,
                Status = createDto.Status,
                CategoryId = createDto.CategoryId,
                LocationId = createDto.LocationId
            };

            _mockMapper.Setup(mapper => mapper.Map<Event>(createDto))
                .Returns(eventEntity);
            _mockEventRepository.Setup(repo => repo.AddAsync(eventEntity))
                .Returns(Task.CompletedTask);
            _mockEventRepository.Setup(repo => repo.GetEventWithDetailsAsync(eventEntity.Id))
                .ReturnsAsync(eventEntity);
            _mockMapper.Setup(mapper => mapper.Map<EventDto>(eventEntity))
                .Returns(expectedDto);
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _eventService.CreateEventAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Title, result.Title);
            Assert.Equal(expectedDto.Description, result.Description);
            Assert.Equal(expectedDto.StartDate, result.StartDate);
            Assert.Equal(expectedDto.EndDate, result.EndDate);
            Assert.Equal(expectedDto.Status, result.Status);
        }

        [Fact]
        public async Task CreateEventAsync_EndDateBeforeStartDate_ThrowsArgumentException()
        {
            // Arrange
            var createDto = new EventCreateDto
            {
                Title = "Invalid Event",
                Description = "Invalid Description",
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(1), // End date before start date
                Status = EventStatus.Scheduled,
                CategoryId = 1,
                LocationId = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _eventService.CreateEventAsync(createDto));
        }

        [Fact]
        public async Task DeleteEventAsync_ExistingId_CallsRemoveAndComplete()
        {
            // Arrange
            var eventId = 1;
            var eventEntity = new Event { Id = eventId };

            _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventId))
                .ReturnsAsync(eventEntity);

            // Act
            await _eventService.DeleteEventAsync(eventId);

            // Assert
            _mockEventRepository.Verify(repo => repo.Remove(eventEntity), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteEventAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var eventId = 999;
            _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventId))
                .ReturnsAsync((Event)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _eventService.DeleteEventAsync(eventId));
        }
    }
}