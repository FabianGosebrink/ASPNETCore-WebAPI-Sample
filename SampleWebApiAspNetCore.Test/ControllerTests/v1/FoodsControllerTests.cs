
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GenFu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using SampleWebApiAspNetCore.v1.Controllers;
using Xunit;

namespace SampleWebApiAspNetCore.Test.ControllerTests.v1
{
    public class FoodsControllerTests
    {
        private FoodsController _controller;
        private Mock<IFoodRepository> _repository;
        private Mock<IUrlHelper> _urlHelper;

        public FoodsControllerTests()
        {
            _repository = new Mock<IFoodRepository>();
            _urlHelper = new Mock<IUrlHelper>();

            _controller = new FoodsController(_urlHelper.Object, _repository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        [Category("Unit Test")]
        public void Get_All_Food_Success()
        {
            // Arrange
            var foods = A.ListOf<FoodItem>(10).AsQueryable();
            _repository.Setup(x => x.GetAll(It.IsAny<QueryParameters>())).Returns(foods);
            _repository.Setup(x => x.Count()).Returns(foods.Count);

            // Act
            var result = _controller.GetAllFoods(new QueryParameters()) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        [Category("Integration Test")]
        public async Task Get_All_Food_Success_Integration_Test()
        {
            using (var httpClient = new TestClientProvider().Client)
            {
                // Arrange

                // Act
                var result = await httpClient.GetAsync("api/v1/Foods?Page=1&PageCount=5");

                // Assert
                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            }
        }
    }
}
