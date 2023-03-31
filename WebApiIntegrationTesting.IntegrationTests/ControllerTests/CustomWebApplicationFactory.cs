using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiIntegrationTesting.DataAccess.Repositories;

namespace WebApiIntegrationTesting.IntegrationTests.ControllerTests
{
    public class CustomWebApplicationFactory: WebApplicationFactory<Program>
    {
        public Mock<IReviewRepository> ReviewRepositoryMock { get; }

        public CustomWebApplicationFactory()
        {
            ReviewRepositoryMock = new Mock<IReviewRepository>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(ReviewRepositoryMock.Object);
            });
        }
    }
}
