using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebApiIntegrationTesting.Controllers;
using WebApiIntegrationTesting.DataAccess.Entities;
using WebApiIntegrationTesting.DataAccess.Repositories;
using Xunit;

namespace WebApiIntegrationTesting.IntegrationTests.ControllerTests
{
    public class BookReviewsControllerTests : IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public BookReviewsControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_Always_ReturnsAllBooks()
        {
            var mockReviews = new BookReview[]
            {
                new(){ Id = 1, Title="A", Rating = 2 },
                new(){ Id = 2, Title="B", Rating = 3 }
            }.AsQueryable();

            _factory.ReviewRepositoryMock.Setup(r => r.AllReviews).Returns(mockReviews);

            var response = await _client.GetAsync("/BookReviews");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var data = JsonConvert.DeserializeObject<IEnumerable<BookReview>>(await response.Content.ReadAsStringAsync());

            Assert.Collection(data,
                r =>
                {
                    Assert.Equal("A", r.Title);
                    Assert.Equal(2, r.Rating);
                },
                r =>
                {
                    Assert.Equal("B", r.Title);
                    Assert.Equal(3, r.Rating);
                }
            );
        }

        [Fact]
        public async Task GetById_IfExists_ReturnsBook()
        {
            var mockReviews = new BookReview[]
            {
                new(){ Id = 1, Title="A", Rating = 2 },
                new(){ Id = 2, Title="B", Rating = 3 }
            }.AsQueryable();

            _factory.ReviewRepositoryMock.Setup(r => r.AllReviews).Returns(mockReviews);

            var response = await _client.GetAsync("/BookReviews/2");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var data = JsonConvert.DeserializeObject<BookReview>(await response.Content.ReadAsStringAsync());


            Assert.Equal(2, data.Id);
            Assert.Equal("B", data.Title);
            Assert.Equal(3, data.Rating);
        }

        [Fact]
        public async Task GetById_IfMissing_Returns404()
        {
            var mockReviews = new BookReview[]
            {
                new(){ Id = 1, Title="A", Rating = 2 },
                new(){ Id = 2, Title="B", Rating = 3 }
            }.AsQueryable();

            _factory.ReviewRepositoryMock.Setup(r => r.AllReviews).Returns(mockReviews);

            var response = await _client.GetAsync("/BookReviews/3");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetSummary_Always_ReturnsSummary()
        {
            var mockReviews = new BookReview[]
            {
                new(){ Id = 1, Title="A", Rating = 1 },
                new(){ Id = 2, Title="B", Rating = 2 },
                new(){ Id = 3, Title="C", Rating = 5 },
                new(){ Id = 4, Title="B", Rating = 4 }

            }.AsQueryable();

            _factory.ReviewRepositoryMock.Setup(r => r.AllReviews).Returns(mockReviews);

            var response = await _client.GetAsync("/BookReviews/summary");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var data = JsonConvert.DeserializeObject<IEnumerable<BookReview>>(await response.Content.ReadAsStringAsync());


            Assert.Collection(data,
                r =>
                {
                    Assert.Equal("A", r.Title);
                    Assert.Equal(1, r.Rating);
                },
                r =>
                {
                    Assert.Equal("B", r.Title);
                    Assert.Equal(3, r.Rating);
                },
                r =>
                {
                    Assert.Equal("C", r.Title);
                    Assert.Equal(5, r.Rating);
                }
            );
        }

        [Fact]
        public async Task Post_WithValidData_SavesReview()
        {
            var newReview = new BookReview { Title = "NewTitle", Rating = 4 };

            _factory.ReviewRepositoryMock.Setup(r => r.Create(It.Is<BookReview>(b => b.Title == "NewTitle" && b.Rating == 4))).Verifiable();
            _factory.ReviewRepositoryMock.Setup(r => r.SaveChanges()).Verifiable();

            var response = await _client.PostAsync("/BookReviews", JsonContent.Create(newReview));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            _factory.ReviewRepositoryMock.VerifyAll();
        }

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
