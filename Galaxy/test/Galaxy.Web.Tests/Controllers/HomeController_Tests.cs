using System.Threading.Tasks;
using Galaxy.Web.Controllers;
using Shouldly;
using Xunit;

namespace Galaxy.Web.Tests.Controllers
{
    public class HomeController_Tests: GalaxyWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}
