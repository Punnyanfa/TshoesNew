
using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.TemplateService;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FCSP.Tests
{
    public class TemplateServiceDeleteTests
    {
        private readonly Mock<ICustomShoeDesignTemplateRepository> _templateRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly TemplateService _templateService;
        public TemplateServiceDeleteTests()
        {
            _templateRepositoryMock = new Mock<ICustomShoeDesignTemplateRepository>();
            _configurationMock = new Mock<IConfiguration>(); 
            _templateService = new TemplateService(_templateRepositoryMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task DeleteTemplate_InvalidId()
        {
            // Arrange
            var request = new DeleteTemplateRequest { Id = 0 };
            _templateRepositoryMock.Setup(x => x.Find(0)).Returns((CustomShoeDesignTemplate)null);

            // Act
            var result = await _templateService.DeleteTemplate(request);

            // Assert
            Assert.Equal(404, result.Code);
            Assert.Equal("Template not found", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task DeleteTemplate_TemplateNotFound()
        {
            // Arrange
            var request = new DeleteTemplateRequest { Id = 12345555 };
            _templateRepositoryMock.Setup(x => x.Find(12345555)).Returns((CustomShoeDesignTemplate)null);

            // Act
            var result = await _templateService.DeleteTemplate(request);

            // Assert
            Assert.Equal(404, result.Code);
            Assert.Equal("Template not found", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task DeleteTemplate_ValidRequest()
        {
            // Arrange
            var template = new CustomShoeDesignTemplate
            {
                Id = 1,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var request = new DeleteTemplateRequest { Id = 1 };
            _templateRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
              .ReturnsAsync(template);
            _templateRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CustomShoeDesignTemplate>())).Returns(Task.CompletedTask);

            // Act
            var result = await _templateService.DeleteTemplate(request);

            // Assert
            Assert.Equal(200, result.Code);
            Assert.Equal("Template deleted successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Success);
            Assert.True(template.IsDeleted);
        }
    }
}

