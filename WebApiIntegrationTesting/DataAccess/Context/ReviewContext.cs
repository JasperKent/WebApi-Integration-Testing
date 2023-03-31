using Microsoft.EntityFrameworkCore;
using WebApiIntegrationTesting.DataAccess.Entities;

namespace WebApiIntegrationTesting.DataAccess.Context
{
    public class ReviewContext : DbContext
    {
        public ReviewContext(DbContextOptions<ReviewContext> options)
            :base(options)
        {            
        }

        public DbSet<BookReview> BookReviews { get; set; }
    }
}
