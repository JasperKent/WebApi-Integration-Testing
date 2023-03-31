using WebApiIntegrationTesting.DataAccess.Context;
using WebApiIntegrationTesting.DataAccess.Entities;

namespace WebApiIntegrationTesting.DataAccess.Repositories
{
    public class SQLiteRepository : IReviewRepository
    {
        private readonly ReviewContext _reviewContext;

        public SQLiteRepository(ReviewContext reviewContext)
        {
            _reviewContext = reviewContext;
        }

        public IQueryable<BookReview> AllReviews => _reviewContext.BookReviews;

        public void Create(BookReview review) => _reviewContext.BookReviews.Add(review);

        public void Remove(BookReview review) => _reviewContext.BookReviews.Remove(review);

        public void SaveChanges() => _reviewContext.SaveChanges();
    }
}
