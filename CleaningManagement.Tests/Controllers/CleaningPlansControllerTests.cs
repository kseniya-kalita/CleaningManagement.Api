using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using CleaningManagement.Api.Boundary.Request;
using CleaningManagement.Api.Controllers;
using CleaningManagement.DAL.Repositories;
using Moq;
using CleaningManagement.DAL;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleaningManagement.Tests.Controllers
{
    public class CleaningPlansControllerTests
    {
        private readonly Mock<ICleaningPlanRepository> _mockedRepository;
        private readonly CleaningPlansController _controller;
        
        public CleaningPlansControllerTests()
        {
            _mockedRepository = new Mock<ICleaningPlanRepository>();
            _controller = new CleaningPlansController(_mockedRepository.Object);

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

            var okResult = await _controller.CreateCleaningPlan(cleaningPlanDto).ConfigureAwait(false) as OkObjectResult;

            okResult.Should().NotBeNull();
            
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            okResult.Value.Should().BeEquivalentTo(expectedCleaningPlan, options => 
                options.Excluding(x => x.CreationDate)
                       .Excluding(x => x.Id)
                );
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

            _mockedRepository.Setup(x => x.GetByCustomerIdAsync(It.IsAny<int>()))
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

            _mockedRepository.Setup(x => x.GetByCustomerIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<CleaningPlan>());
            
            var notFoundResult = await _controller.GetCleaningPlansByCustomerId(customerId).ConfigureAwait(false) as NotFoundResult;

            notFoundResult.Should().NotBeNull();

            notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
        
        [Fact]
        public async Task UpdateCleaningPlanById_WithValiObject_ReturnsNoContent()
        {
            int customerId = 123223;
            var id = new Guid("6c442581-c67a-41e5-8d2d-b1176de31087");
            var cleaningPlanDto = CreateCleaningPlanDto(customerId);
            var expectedResult = new CleaningPlan
            {
                Id = id,
                Title = "Test title",
                Description = "Test description",
                CreationDate = DateTime.UtcNow.AddHours(-1),
                CustomerId = customerId
            };

            _mockedRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expectedResult);
            _mockedRepository.Setup(x => x.UpdateAsync(It.IsAny<CleaningPlan>()));
            
            var noContentResult = await _controller.UpdateCleaningPlanById(id, cleaningPlanDto).ConfigureAwait(false) as NoContentResult;

            noContentResult.Should().NotBeNull();

            noContentResult.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }
        
        [Fact]
        public async Task UpdateCleaningPlanById_WithNullObject_ReturnsBadRequest()
        {
            var id = new Guid("6c442581-c67a-41e5-8d2d-b1176de31087");

            _mockedRepository.Setup(x => x.UpdateAsync(It.IsAny<CleaningPlan>()));
            
            var noContentResult = await _controller.UpdateCleaningPlanById(id, null).ConfigureAwait(false) as BadRequestResult;

            noContentResult.Should().NotBeNull();

            noContentResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
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
    }
}
