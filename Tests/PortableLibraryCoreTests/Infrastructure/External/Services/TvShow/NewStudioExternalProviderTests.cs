using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibraryCoreTests.Infrastructure.External.Services.TvShow
{
    public class NewStudioExternalProviderTests
    {
        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public NewStudioExternalProviderTests()
        {
            _retryService = new RetryService();
        }

        #endregion
        
        #region Tests

        [Fact]
        public async Task Should_Extract_Sherlock()
        {
            var service = new NewStudioExternalProvider(_retryService);
            
            var model = await service.ExtractTvShow("http://newstudio.tv/viewforum.php?f=152");
        }

        #endregion
    }
}