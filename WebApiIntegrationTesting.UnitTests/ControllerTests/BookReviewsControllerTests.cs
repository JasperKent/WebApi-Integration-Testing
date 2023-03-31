using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiIntegrationTesting.Controllers;
using WebApiIntegrationTesting.DataAccess.Entities;
using WebApiIntegrationTesting.DataAccess.Repositories;
using Xunit;

namespace WebApiIntegrationTesting.UnitTests.ControllerTests
{
    public class BookReviewsControllerTests
    {
        [Fact]
        public void Get_Always_ReturnsAllBooks()
        {
            var mockReviews = new BookReview[]
            {
                new(){ Id = 1, Title="A", Rating = 2 },
                new(){ Id = 2, Title="B", Rating = 3 }
            }.AsQueryable();

            var mockRepository = new Mock<IReviewRepository>();

            mockRepository.Setup(r => r.AllReviews).Returns(mockReviews);

            var controller = new BookReviewsController(mockRepository.Object);

            var result = controller.Get().Result as OkObjectResult;

            Assert.NotNull(result);

            Assert.Collection((result.Value as IEnumerable<BookReview>)!,
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
        public void GetById_IfExists_ReturnsBook()
        {
            var mockReviews = new BookReview[]
            {
                new(){ Id = 1, Title="A", Rating = 2 },
                new(){ Id = 2, Title="B", Rating = 3 }
            }.AsQueryable();

            var mockRepository = new Mock<IReviewRepository>();

            mockRepository.Setup(r => r.AllReviews).Returns(mockReviews);

            var controller = new BookReviewsController(mockRepository.Object);

            var result = controller.Get(2).Result as OkObjectResult;

            Assert.NotNull(result);

            var book = result.Value as BookReview;

            Assert.Equal(2, book!.Id);
            Assert.Equal("B", book!.Title);
            Assert.Equal(3, book!.Rating);
        }

        [Fact]
        public void GetById_IfMissing_Returns404()
        {
            var mockReviews = new BookReview[]
            {
                new(){ Id = 1, Title="A", Rating = 2 },
                new(){ Id = 2, Title="B", Rating = 3 }
            }.AsQueryable();

            var mockRepository = new Mock<IReviewRepository>();

            mockRepository.Setup(r => r.AllReviews).Returns(mockReviews);

            var controller = new BookReviewsController(mockRepository.Object);

            var result = controller.Get(3).Result;

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetSummary_Always_ReturnsSummary()
        {
            var mockReviews = new BookReview[]
            {
                new(){ Id = 1, Title="A", Rating = 1 },
                new(){ Id = 2, Title="B", Rating = 2 },
                new(){ Id = 3, Title="C", Rating = 5 },
                new(){ Id = 4, Title="B", Rating = 4 }

            }.AsQueryable();

            var mockRepository = new Mock<IReviewRepository>();

            mockRepository.Setup(r => r.AllReviews).Returns(mockReviews);

            var controller = new BookReviewsController(mockRepository.Object);

            var result = controller.Summary().Result as OkObjectResult;

            Assert.NotNull(result);

            Assert.Collection((result.Value as IEnumerable<BookReview>)!,
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
        public void Post_WithValidData_SavesReview()
        {
            var newReview = new BookReview { Title = "NewTitle", Rating = 4 };

            var mockRepository = new Mock<IReviewRepository>();

            mockRepository.Setup(r => r.Create(It.Is<BookReview>(b => b.Title == "NewTitle" && b.Rating == 4))).Verifiable();
            mockRepository.Setup(r => r.SaveChanges()).Verifiable();

            var controller = new BookReviewsController(mockRepository.Object);

            var result = controller.Post(newReview).Result as CreatedAtActionResult;

            Assert.NotNull(result);

            mockRepository.VerifyAll();
        }
    }
}
