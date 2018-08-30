using System.Threading.Tasks;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.MyShows
{
    public class AccessTests : MyShowsTestsBase
    {
        #region Token

        [Fact]
        public async Task Should_Get_Access_Token()
        {
            string token = await EnglishService.GetToken();

            Assert.True(!string.IsNullOrWhiteSpace(token));
        }

        #endregion
    }
}
