using Moq;
using Xunit;
using ShopperBackend.Controllers;
using ShopperBackend.Models;
using ShopperBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopperBackend.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductsService> _mockProductsService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockProductsService = new Mock<IProductsService>();
            _controller = new ProductsController(_mockProductsService.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = "1", Name = "Product1" }, new Product { Id = "2", Name = "Product2" } };
            _mockProductsService.Setup(service => service.GetAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, returnProducts.Count);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductsService.Setup(service => service.GetAsync("1")).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProduct("1");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetProduct_ReturnsOkResult_WithProduct()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "Product1" };
            _mockProductsService.Setup(service => service.GetAsync("1")).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("1", returnProduct.Id);
            Assert.Equal("Product1", returnProduct.Name);
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedAtAction_WithProduct()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "Product1" };

            // Act
            var result = await _controller.CreateProduct(product);

            // Assert
            var createdAtActionResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            var returnProduct = Assert.IsType<Product>(createdAtActionResult.Value);
            Assert.Equal(product.Id, returnProduct.Id);
            Assert.Equal(product.Name, returnProduct.Name);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNoContent_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "UpdatedProduct" };
            _mockProductsService.Setup(service => service.GetAsync("1")).ReturnsAsync(product);

            // Act
            var result = await _controller.UpdateProduct("1", product);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductsService.Setup(service => service.GetAsync("1")).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.UpdateProduct("1", new Product());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "Product1" };
            _mockProductsService.Setup(service => service.GetAsync("1")).ReturnsAsync(product);

            // Act
            var result = await _controller.DeleteProduct("1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductsService.Setup(service => service.GetAsync("1")).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.DeleteProduct("1");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
