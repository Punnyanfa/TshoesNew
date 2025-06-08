using FCSP.Common.Enums;
using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.TemplateService;
using Microsoft.Extensions.Configuration;
using Moq;
using FCSP.Common.Utils;

namespace FCSP.Tests
{
    public class TemplateServiceUpdateStatusTests
    {
        private readonly Mock<ICustomShoeDesignTemplateRepository> _templateRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly TemplateService _templateService;
            public TemplateServiceUpdateStatusTests()
            {
                _templateRepositoryMock = new Mock<ICustomShoeDesignTemplateRepository>();
                _configurationMock = new Mock<IConfiguration>();
                _templateService = new TemplateService(_templateRepositoryMock.Object, _configurationMock.Object);
            }

        [Fact]
        public async Task UpdateTemplateStatus_TemplateNotFound()
        {
            // Arrange
            var request = new UpdateTemplateStatusRequest { Id = 12345555, Status = TemplateStatus.Public };
            _templateRepositoryMock.Setup(x => x.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((CustomShoeDesignTemplate)null);

            // Act
            var result = await _templateService.UpdateTemplateStatus(request);

            // Assert
            Assert.Equal(404, result.Code);
            Assert.Equal("Template not found", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateTemplateStatus_ValidStatusPublic()
        {
            var template = new CustomShoeDesignTemplate
            {
                Id = 1,
                Status = TemplateStatus.Private,
                UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7()
            };
            var request = new UpdateTemplateStatusRequest { Id = 1, Status = TemplateStatus.Public };
            _templateRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
                .ReturnsAsync(template);
            _templateRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CustomShoeDesignTemplate>())).Returns(Task.CompletedTask);

            var result = await _templateService.UpdateTemplateStatus(request);

            Assert.Equal(200, result.Code);
            Assert.Equal("Template restored successfully", result.Message);
        }

        [Fact]
        public async Task UpdateTemplateStatus_ValidStatusPrivate()
        {
            // Arrange
            var template = new CustomShoeDesignTemplate
            {
                Id = 1,
                Status = TemplateStatus.Public,
                UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7()
            };
            var request = new UpdateTemplateStatusRequest { Id = 1, Status = TemplateStatus.Private };
            _templateRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
                .ReturnsAsync(template);
            _templateRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CustomShoeDesignTemplate>())).Returns(Task.CompletedTask);

            // Act
            var result = await _templateService.UpdateTemplateStatus(request);

            // Assert
            Assert.Equal(200, result.Code);
            Assert.Equal("Template restored successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Success);
            Assert.Equal(TemplateStatus.Private, template.Status);
        }

        [Fact]
        public async Task UpdateTemplateStatus_InvalidStatus()
        {
            // Arrange
            var template = new CustomShoeDesignTemplate
            {
                Id = 1,
                Status = TemplateStatus.Public,
                UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7()
            };
            var request = new UpdateTemplateStatusRequest { Id = 1, Status = (TemplateStatus)3 };
            _templateRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
                .ReturnsAsync(template);
            _templateRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CustomShoeDesignTemplate>())).Returns(Task.CompletedTask);

            // Act
            var result = await _templateService.UpdateTemplateStatus(request);

            // Assert
            Assert.Equal(400, result.Code);
            Assert.Equal("status is invalid", result.Message);
            Assert.Null(result.Data);
        }
    }
}


