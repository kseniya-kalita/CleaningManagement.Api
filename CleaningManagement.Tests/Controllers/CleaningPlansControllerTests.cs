using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using CleaningManagement.Abstractions.Dtos;
using CleaningManagement.Api.Controllers;
using CleaningManagement.Abstractions.Interfaces;
using Moq;
using CleaningManagement.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FluentAssertions.Extensions;

namespace CleaningManagement.Tests.Controllers
{
    public class CleaningPlansControllerTests
    {
        private readonly Mock<ICleaningManagementService> _mockedService;
        private readonly CleaningPlansController _controller;
        
        public CleaningPlansControllerTests()
        {
            _mockedService = new Mock<ICleaningManagementService>();
            _controller = new CleaningPlansController(_mockedService.Object);

        }

        [Fact]
        public async Task CreateCleaningPlan_WithValidObject_ReturnsOkObjectResult()
        {
            var customerId = 123223;
            var cleaningPlanDto = CreateCleaningPlanDto(customerId);
            var expectedCleaningPlan = new CleaningPlan
            {
                Title = cleaningPlanDto.Title,
                Description = cleaningPlanDto.Description,
                CustomerId = cleaningPlanDto.CustomerId,
            };

            _mockedService.Setup(x => x.CreateAsync(It.IsAny<CleaningPlanForManipulationDto>()))
                .ReturnsAsync(expectedCleaningPlan);

            var okResult = await _controller.CreateCleaningPlan(cleaningPlanDto).ConfigureAwait(false) as ObjectResult;

            okResult.Should().NotBeNull();
            
            okResult.StatusCode.Should().Be((int)HttpStatusCode.Created);

            var response = okResult.Value as CleaningPlan;
            response.Should().BeEquivalentTo(expectedCleaningPlan, options =>
                    options.Excluding(x => x.CreationDate)
                           .Excluding(x => x.Id)
            );
            response.CreationDate.Should().BeCloseTo(expectedCleaningPlan.CreationDate, 1.Seconds());
        }

        [Fact]
        public async Task CreateCleaningPlan_WithInvalidObject_ReturnsBadRequestResult()
        {
            var cleaningPlanDto = new CleaningPlanForManipulationDto
            {
                CustomerId = 123223,
                Description = "This plan is meant to be used for double bed rooms."
            };

            _controller.ModelState.AddModelError("Title", "'Title' cannot be null.");

            var badRequestResult = await _controller.CreateCleaningPlan(cleaningPlanDto).ConfigureAwait(false) as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();

            badRequestResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateCleaningPlan_WithNullObject_ReturnsBadRequest()
        {
            var badRequestResult = await _controller.CreateCleaningPlan(null).ConfigureAwait(false) as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();

            badRequestResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            badRequestResult.Value.Should().Be("Cleaning plan model cannot be null");
        }

        [Fact]
        public async Task GetCleaningPlansByCustomerId_WithExistingCustomerId_ReturnsOkObjectResult()
        {
            int customerId = 123223;
            var expectedList = CreateCleaningPlanList(customerId);

            _mockedService.Setup(x => x.GetByCustomerIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedList);

            var okResult = await _controller.GetCleaningPlansByCustomerId(customerId).ConfigureAwait(false) as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            okResult.Value.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public async Task GetCleaningPlansByCustomerId_WithNotExistingCustomerId_ReturnsNotFound()
        {
            int customerId = 123223;
            List<CleaningPlan> expectedList = null;

            _mockedService.Setup(x => x.GetByCustomerIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedList);

            var notFoundResult = await _controller.GetCleaningPlansByCustomerId(customerId).ConfigureAwait(false) as NotFoundResult;

            notFoundResult.Should().NotBeNull();

            notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateCleaningPlanById_WithValidObject_ReturnsNoContent()
        {
            int customerId = 123223;
            var id = new Guid("6c442581-c67a-41e5-8d2d-b1176de31087");
            var cleaningPlanDto = CreateCleaningPlanDto(customerId);
            var expectedResult = CreateCleaningPlan(customerId, id);

            _mockedService.Setup(x => x.UpdateByIdAsync(It.IsAny<Guid>(), cleaningPlanDto)).ReturnsAsync(expectedResult);

            var noContentResult = await _controller.UpdateCleaningPlanById(id, cleaningPlanDto).ConfigureAwait(false) as NoContentResult;

            noContentResult.Should().NotBeNull();

            noContentResult.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateCleaningPlanById_WithNullObject_ReturnsBadRequest()
        {
            var id = new Guid("6c442581-c67a-41e5-8d2d-b1176de31087");

            var noContentResult = await _controller.UpdateCleaningPlanById(id, null).ConfigureAwait(false) as BadRequestResult;

            noContentResult.Should().NotBeNull();

            noContentResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateCleaningPlanById_WithNotExistingObject_ReturnsNotFound()
        {
            int customerId = 123223;
            var id = new Guid("6c442581-c67a-41e5-8d2d-b1176de31087");
            var cleaningPlanDto = CreateCleaningPlanDto(customerId);
            CleaningPlan expectedResult = null;

            _mockedService.Setup(x => x.UpdateByIdAsync(It.IsAny<Guid>(), cleaningPlanDto)).ReturnsAsync(expectedResult);

            var noContentResult = await _controller.UpdateCleaningPlanById(id, cleaningPlanDto).ConfigureAwait(false) as NotFoundResult;

            noContentResult.Should().NotBeNull();

            noContentResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteCleaningPlanById_WithExistingId_ReturnsNoContent()
        {
            var expectedResult = CreateCleaningPlan();

            _mockedService.Setup(x => x.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expectedResult);

            var noContentResult = await _controller.DeleteCleaningPlanById(expectedResult.Id).ConfigureAwait(false) as NoContentResult;

            noContentResult.Should().NotBeNull();

            noContentResult.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteCleaningPlanById_WithNotexistingId_ReturnsNotFound()
        {
            CleaningPlan expectedResult = null;
            var id = Guid.NewGuid();
            _mockedService.Setup(x => x.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expectedResult);

            var noContentResult = await _controller.DeleteCleaningPlanById(id).ConfigureAwait(false) as NotFoundResult;

            noContentResult.Should().NotBeNull();

            noContentResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        private CleaningPlanForManipulationDto CreateCleaningPlanDto(int customerId)
        {
            return new CleaningPlanForManipulationDto
            {
                Title = "Hotel Room Cleaning, double bed",
                CustomerId = customerId,
                Description = "This plan is meant to be used for double bed rooms."
            };
        }

        private List<CleaningPlan> CreateCleaningPlanList(int customerId)
        {
            return new List<CleaningPlan>
            {
                new CleaningPlan
                {
                    Id = new Guid("8c442581-c67a-41e5-8d2d-b1176de31087"),
                    Title = "Hotel Room Cleaning, double bed",
                    CustomerId = customerId,
                    Description = "This plan is meant to be used for double bed rooms.",
                    CreationDate = DateTime.UtcNow
                },
                new CleaningPlan
                {
                    Id = new Guid("9c442581-c67a-41e5-8d2d-b1176de31087"),
                    Title = "Mall Cleaning, inner city",
                    CustomerId = customerId,
                    Description = "Suitable only for malls smaller than 23000 m².",
                    CreationDate = DateTime.UtcNow
                }
            };
        }
        
        private CleaningPlan CreateCleaningPlan(int? customerId = null, Guid? id = null)
        {
            return new CleaningPlan
            {
                Id = id ?? new Guid("6c442581-c67a-41e5-8d2d-b1176de31087"),
                Title = "Test title",
                Description = "Test description",
                CreationDate = DateTime.UtcNow.AddHours(-1),
                CustomerId = customerId ?? 123223
            };
        }
    }
}
