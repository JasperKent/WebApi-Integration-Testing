using WebApiIntegrationTesting.DataAccess.Entities;

namespace WebApiIntegrationTesting.DataAccess.Repositories
{
    public interface IReviewRepository
    {
        public IQueryable<BookReview> AllReviews { get; }

        void Create(BookReview review);
        void Remove(BookReview result);
        void SaveChanges();
    }
}
