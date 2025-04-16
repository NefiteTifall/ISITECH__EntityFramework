using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ISITECH__EventsArea.Application.Services;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using Moq;
using Xunit;

namespace ISITECH__EventsArea.Tests.Services
{
    public class LocationServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly Mock<IRoomRepository> _mockRoomRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly LocationService _locationService;

        public LocationServiceTests()
        {
            _mockLocationRepository = new Mock<ILocationRepository>();
            _mockEventRepository = new Mock<IEventRepository>();
            _mockRoomRepository = new Mock<IRoomRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(uow => uow.Locations).Returns(_mockLocationRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Events).Returns(_mockEventRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Rooms).Returns(_mockRoomRepository.Object);
            _locationService = new LocationService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetLocationByIdAsync_ExistingId_ReturnsLocation()
        {
            // Arrange
            var locationId = 1;
            var location = new Location
            {
                Id = locationId,
                Name = "Test Location",
                Address = "123 Test St",
                City = "Test City",
                Country = "Test Country",
                Capacity = 100
            };

            _mockLocationRepository.Setup(repo => repo.GetLocationWithDetailsAsync(locationId))
                .ReturnsAsync(location);

            // Act
            var result = await _locationService.GetLocationByIdAsync(locationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(locationId, result.Id);
            Assert.Equal("Test Location", result.Name);
            Assert.Equal("123 Test St", result.Address);
            Assert.Equal("Test City", result.City);
            Assert.Equal("Test Country", result.Country);
            Assert.Equal(100, result.Capacity);
        }

        [Fact]
        public async Task CreateLocationAsync_ValidLocation_ReturnsCreatedLocation()
        {
            // Arrange
            var location = new Location
            {
                Name = "New Location",
                Address = "456 New St",
                City = "New City",
                Country = "New Country",
                Capacity = 200
            };

            _mockLocationRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Func<Location, bool>>()))
                .ReturnsAsync((Location)null);
            _mockLocationRepository.Setup(repo => repo.AddAsync(location))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _locationService.CreateLocationAsync(location);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Location", result.Name);
            Assert.Equal("456 New St", result.Address);
            Assert.Equal("New City", result.City);
            Assert.Equal("New Country", result.Country);
            Assert.Equal(200, result.Capacity);
            _mockLocationRepository.Verify(repo => repo.AddAsync(location), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateLocationAsync_MissingName_ThrowsArgumentException()
        {
            // Arrange
            var location = new Location
            {
                Name = "", // Empty name
                Address = "456 New St",
                City = "New City",
                Country = "New Country",
                Capacity = 200
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _locationService.CreateLocationAsync(location));
            Assert.Contains("name is required", exception.Message);
        }

        [Fact]
        public async Task CreateLocationAsync_DuplicateLocation_ThrowsArgumentException()
        {
            // Arrange
            var location = new Location
            {
                Name = "Existing Location",
                Address = "123 Existing St",
                City = "Existing City",
                Country = "Existing Country",
                Capacity = 100
            };

            var existingLocation = new Location
            {
                Id = 1,
                Name = "Existing Location",
                Address = "123 Existing St",
                City = "Existing City",
                Country = "Existing Country",
                Capacity = 100
            };

            _mockLocationRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Func<Location, bool>>()))
                .ReturnsAsync(existingLocation);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _locationService.CreateLocationAsync(location));
            Assert.Contains("already exists", exception.Message);
        }

        [Fact]
        public async Task DeleteLocationAsync_LocationWithEvents_ThrowsInvalidOperationException()
        {
            // Arrange
            var locationId = 1;
            var location = new Location
            {
                Id = locationId,
                Name = "Test Location",
                Address = "123 Test St",
                City = "Test City",
                Country = "Test Country",
                Capacity = 100,
                Events = new List<Event> { new Event { Id = 1, Title = "Test Event" } }
            };

            _mockLocationRepository.Setup(repo => repo.GetLocationWithDetailsAsync(locationId))
                .ReturnsAsync(location);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _locationService.DeleteLocationAsync(locationId));
            Assert.Contains("has associated events", exception.Message);
        }

        [Fact]
        public async Task DeleteLocationAsync_LocationWithRooms_ThrowsInvalidOperationException()
        {
            // Arrange
            var locationId = 1;
            var location = new Location
            {
                Id = locationId,
                Name = "Test Location",
                Address = "123 Test St",
                City = "Test City",
                Country = "Test Country",
                Capacity = 100,
                Events = new List<Event>(),
                Rooms = new List<Room> { new Room { Id = 1, Name = "Test Room" } }
            };

            _mockLocationRepository.Setup(repo => repo.GetLocationWithDetailsAsync(locationId))
                .ReturnsAsync(location);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _locationService.DeleteLocationAsync(locationId));
            Assert.Contains("has associated rooms", exception.Message);
        }

        [Fact]
        public async Task DeleteLocationAsync_ValidLocation_CallsRemoveAndComplete()
        {
            // Arrange
            var locationId = 1;
            var location = new Location
            {
                Id = locationId,
                Name = "Test Location",
                Address = "123 Test St",
                City = "Test City",
                Country = "Test Country",
                Capacity = 100,
                Events = new List<Event>(),
                Rooms = new List<Room>()
            };

            _mockLocationRepository.Setup(repo => repo.GetLocationWithDetailsAsync(locationId))
                .ReturnsAsync(location);

            // Act
            await _locationService.DeleteLocationAsync(locationId);

            // Assert
            _mockLocationRepository.Verify(repo => repo.Remove(location), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }
    }
}