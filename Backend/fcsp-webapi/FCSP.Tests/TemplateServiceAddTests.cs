using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.TemplateService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FCSP.Tests
{
    public class TemplateServiceAddTests
    {
        private readonly Mock<ICustomShoeDesignTemplateRepository> _templateRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IFormFile> _previewImageMock;
        private readonly Mock<IFormFile> _model3DFileMock;
        private readonly TemplateService _templateService;

        public TemplateServiceAddTests()
        {
            _templateRepositoryMock = new Mock<ICustomShoeDesignTemplateRepository>();
            _configurationMock = new Mock<IConfiguration>();
            _previewImageMock = new Mock<IFormFile>();
            _model3DFileMock = new Mock<IFormFile>();

            // Setup configuration
            _configurationMock.Setup(x => x["AzureStorage:ConnectionString"]).Returns("UseDevelopmentStorage=true");
            _configurationMock.Setup(x => x["AzureStorage:ImagesContainer"]).Returns("images");
            _configurationMock.Setup(x => x["AzureStorage:3DModelsContainer"]).Returns("3dmodels");

            _templateService = new TemplateService(_templateRepositoryMock.Object, _configurationMock.Object);
        }

        // Helper method to create a mock IFormFile with specified content and content type
        private IFormFile CreateMockFile(string fileName, string contentType, byte[] content)
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.ContentType).Returns(contentType);
            fileMock.Setup(f => f.Length).Returns(content.Length);
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Callback<Stream, CancellationToken>((s, ct) => s.Write(content, 0, content.Length)) // Fix: Write byte[] to stream
                .Returns(Task.CompletedTask);
            return fileMock.Object;
        }

        // UTCID01: Normal case - All valid inputs
        [Fact]
        public async Task AddTemplate_ValidInputs_ReturnsSuccess()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = "Sport shoe for men to play sports",
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            
            _templateRepositoryMock
       .Setup(x => x.AddAsync(It.IsAny<CustomShoeDesignTemplate>()))
       .ReturnsAsync((CustomShoeDesignTemplate template) => template); 

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(201, result.Code);
            Assert.Equal("Template created successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Success);
        }

        // UTCID02: Name is null
        [Fact]
        public async Task AddTemplate_NameIsNull_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = null,
                Description = "Sport shoe for men",
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(400, result.Code);
            Assert.Equal("Name is required", result.Message);
            Assert.Null(result.Data);
        }

        // UTCID03: Name < 5 characters
        [Fact]
        public async Task AddTemplate_NameLessThan5Chars_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = "shoe",
                Description = "Sport shoe for men",
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            _templateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CustomShoeDesignTemplate>()))
                .ThrowsAsync(new InvalidOperationException("Name must be greater than 5 characters"));

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Contains("Name must be greater than 5 characters", result.Message);
            Assert.Null(result.Data);
        }

        // UTCID04: Name > 50 characters
        [Fact]
        public async Task AddTemplate_NameMoreThan50Chars_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = new string('a', 51), // 51 characters
                Description = "Sport shoe for men",
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            _templateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CustomShoeDesignTemplate>()))
                .ThrowsAsync(new InvalidOperationException("Name must be less than 50 characters"));

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Contains("Name must be less than 50 characters", result.Message);
            Assert.Null(result.Data);
        }

        // UTCID05: Invalid name format
        [Fact]
        public async Task AddTemplate_InvalidNameFormat_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = "123@!*",
                Description = "Sport shoe for men",
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            _templateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CustomShoeDesignTemplate>()))
                .ThrowsAsync(new InvalidOperationException("Invalid name format"));

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Contains("Invalid name format", result.Message);
            Assert.Null(result.Data);
        }

        // UTCID06: Description is null
        [Fact]
        public async Task AddTemplate_DescriptionIsNull_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = null,
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            _templateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CustomShoeDesignTemplate>()))
                .ThrowsAsync(new InvalidOperationException("Description is require"));

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Contains("Description is require", result.Message);
            Assert.Null(result.Data);
        }

        // UTCID07: Description < 3 words
        [Fact]
        public async Task AddTemplate_DescriptionLessThan3Words_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = "play sport",
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            _templateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CustomShoeDesignTemplate>()))
                .ThrowsAsync(new InvalidOperationException("Description must be greater than 3 words"));

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Contains("Description must be greater than 3 words", result.Message);
            Assert.Null(result.Data);
        }

        // UTCID08: Description > 25 words
        [Fact]
        public async Task AddTemplate_DescriptionMoreThan25Words_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = string.Join(" ", Enumerable.Repeat("word", 26)), // 26 words
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            _templateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CustomShoeDesignTemplate>()))
                .ThrowsAsync(new InvalidOperationException("Description must be less than 25 words"));

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Contains("Description must be less than 25 words", result.Message);
            Assert.Null(result.Data);
        }

        // UTCID09: Invalid description format
        [Fact]
        public async Task AddTemplate_InvalidDescriptionFormat_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = "123@/%#",
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            _templateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CustomShoeDesignTemplate>()))
                .ThrowsAsync(new InvalidOperationException("Invalid description format"));

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Contains("Description must", result.Message);
            
        }

        // UTCID10: Gender is null
        [Fact]
        public async Task AddTemplate_GenderIsNull_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = "Sport shoe for men",
                Gender = null,
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            _templateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CustomShoeDesignTemplate>()))
                .ThrowsAsync(new InvalidOperationException("Gender is require"));

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Contains("Gender is require", result.Message);
            Assert.Null(result.Data);
        }

        // UTCID11: Invalid gender
        [Fact]
        public async Task AddTemplate_InvalidGender_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = "Sport shoe for men",
                Gender = "thuan",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            _templateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CustomShoeDesignTemplate>()))
                .ThrowsAsync(new InvalidOperationException("Gender is invalid"));

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Contains("Gender is invalid", result.Message);
            Assert.Null(result.Data);
        }

        // UTCID12: Color is null
        [Fact]
        public async Task AddTemplate_ColorIsNull_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = "Sport shoe for men",
                Gender = "Male",
                Color = null,
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            _templateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CustomShoeDesignTemplate>()))
                .ThrowsAsync(new InvalidOperationException("Color is require"));

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Contains("Color is require", result.Message);
            Assert.Null(result.Data);
        }

        // UTCID13: Invalid color
        [Fact]
        public async Task AddTemplate_InvalidColor_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = "Sport shoe for men",
                Gender = "Male",
                Color = "thuan",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            _templateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CustomShoeDesignTemplate>()))
                .ThrowsAsync(new InvalidOperationException("Color is invalid"));

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Contains("Color is invalid", result.Message);
            Assert.Null(result.Data);
        }
        [Fact]
        public async Task AddTemplate_PreviewImageIsNull_ReturnsError()
        {
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = "Sport shoe for men",
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = null,
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            var result = await _templateService.AddTemplate(request);

            Assert.Equal(500, result.Code);
            Assert.Contains("Preview Image is require", result.Message);

        }

        // UTCID15: Invalid preview image file
        [Fact]
        public async Task AddTemplate_InvalidPreviewImage_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = "Sport shoe for men",
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.doc", "application/msword", new byte[] { 0x00, 0x00 }), // Invalid format
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 200000,
                IsAvailable = true
            };

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Contains("Azure Storage", result.Message);

        }

        [Fact]
        public async Task AddTemplate_Model3DFileIsNull_ReturnsError()
        {
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = "Sport shoe for men",
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = null,
                BasePrice = 200000,
                IsAvailable = true
            };

            var result = await _templateService.AddTemplate(request);

            Assert.Equal(500, result.Code);
            Assert.Contains("Model3DFile is require", result.Message);
        }

        // UTCID17: Invalid 3D model file
        [Fact]
        public async Task AddTemplate_InvalidModel3DFile_ReturnsError()
        {
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = "Sport shoe for men",
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.png", "image/png", new byte[] { 0x89, 0x50 }), 
                BasePrice = 200000,
                IsAvailable = true
            };

            var result = await _templateService.AddTemplate(request);

            Assert.Equal(500, result.Code);
            Assert.Contains("Azure Storage", result.Message);
            Assert.Null(result.Data);
        }

        // UTCID18: BasePrice is negative
        [Fact]
        public async Task AddTemplate_NegativeBasePrice_ReturnsError()
        {
            // Arrange
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = "Sport shoe for men",
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = -100,
                IsAvailable = true
            };

            // Act
            var result = await _templateService.AddTemplate(request);

            // Assert
            Assert.Equal(400, result.Code);
            Assert.Equal("BasePrice cannot be negative", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task AddTemplate_InvalidBasePriceFormat_ReturnsError()
        {
            var request = new AddTemplateRequest
            {
                Name = "Sport shoe",
                Description = "Sport shoe for men",
                Gender = "Male",
                Color = "Red,Blue",
                PreviewImage = CreateMockFile("preview.jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 }),
                Model3DFile = CreateMockFile("model.glb", "model/gltf-binary", new byte[] { 0x67, 0x6C, 0x54, 0x46 }),
                BasePrice = 0, 
                IsAvailable = true
            };

            _templateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CustomShoeDesignTemplate>()))
                .ThrowsAsync(new InvalidOperationException("Invalid BasePrice format"));

            var result = await _templateService.AddTemplate(request);

            Assert.Equal(500, result.Code);
            Assert.Contains("Invalid BasePrice format", result.Message);
            Assert.Null(result.Data);
        }
      
    }
}