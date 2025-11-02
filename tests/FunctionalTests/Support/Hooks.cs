using FunctionalTests.Drivers;

namespace FunctionalTests.Support
{
    [Binding]
    public class Hooks
    {
        private readonly TestContext _testContext;
        private static ApiClient? _sharedApiClient;

        public Hooks(TestContext testContext)
        {
            _testContext = testContext;
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            // Cria uma instância compartilhada do ApiClient para a feature
            _sharedApiClient = new ApiClient();
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            // Limpa o ApiClient após a feature
            _sharedApiClient?.Dispose();
            _sharedApiClient = null;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Limpa o contexto antes de cada cenário
            _testContext.Response = null;
            _testContext.ResponseBody = null;
            _testContext.Token = null;
            _testContext.LoginRequest = null;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Limpa recursos após o cenário
            _testContext.Response?.Dispose();
            
            // Limpa autenticação do cliente
            if (_sharedApiClient != null)
            {
                _sharedApiClient.ClearAuthentication();
            }
        }

        /// <summary>
        /// Obtém a instância compartilhada do ApiClient
        /// </summary>
        public static ApiClient GetApiClient()
        {
            return _sharedApiClient ?? new ApiClient();
        }
    }
}

