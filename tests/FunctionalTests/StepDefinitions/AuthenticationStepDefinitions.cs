using System.Net;
using System.Text.Json;
using FluentAssertions;
using FunctionalTests.Drivers;
using FunctionalTests.Support;

namespace FunctionalTests.StepDefinitions
{
    [Binding]
    public sealed class AuthenticationStepDefinitions
    {
        private readonly TestContext _testContext;
        private ApiClient? _apiClient;

        public AuthenticationStepDefinitions(TestContext testContext)
        {
            _testContext = testContext;
        }

        private ApiClient GetApiClient()
        {
            return _apiClient ??= new ApiClient();
        }

        [Given("que eu tenho um usuário cadastrado com:")]
        public void GivenQueEuTenhoUmUsuarioCadastradoCom(Table table)
        {
            var row = table.Rows.First();
            var email = row["Email"];
            var password = row["Password"];

            _testContext.LoginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };
        }

        [When(@"eu faço login com email ""([^""]*)"" e senha ""([^""]*)""")]
        public async Task WhenEuFacoLoginComEmailESenha(string email, string password)
        {
            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };

            _testContext.LoginRequest = loginRequest;
            _testContext.Response = await GetApiClient().LoginAsync(loginRequest);
            _testContext.ResponseBody = await _testContext.Response.Content.ReadAsStringAsync();
        }

        [Then(@"a resposta deve ter status code (\d+)")]
        public void ThenARespostaDeveTerStatusCode(int expectedStatusCode)
        {
            _testContext.StatusCode.Should().Be((HttpStatusCode)expectedStatusCode,
                $"esperava-se status code {expectedStatusCode}, mas recebeu {_testContext.StatusCode}");
        }

        [Then(@"a resposta deve conter um token JWT")]
        public void ThenARespostaDeveConterUmTokenJWT()
        {
            _testContext.ResponseBody.Should().NotBeNullOrEmpty("a resposta deve conter um corpo");
            
            var responseJson = JsonDocument.Parse(_testContext.ResponseBody!);
            var root = responseJson.RootElement;
            
            root.TryGetProperty("token", out var tokenProperty).Should().BeTrue("a resposta deve conter a propriedade 'token'");
            tokenProperty.GetString().Should().NotBeNullOrEmpty("o token não deve estar vazio");

            _testContext.Token = tokenProperty.GetString();
        }

        [Then(@"a resposta deve conter a mensagem ""([^""]*)""")]
        public void ThenARespostaDeveConterAMensagem(string expectedMessage)
        {
            _testContext.ResponseBody.Should().Contain(expectedMessage,
                $"a resposta deve conter a mensagem '{expectedMessage}'");
        }

        [Then(@"a resposta não deve conter um token")]
        public void ThenARespostaNaoDeveConterUmToken()
        {
            var body = _testContext.ResponseBody?.Trim();

            if (string.IsNullOrWhiteSpace(body) || !body.StartsWith("{"))
            {
                body.Should().NotContain("token", "não deve haver token em respostas de erro simples");
                return;
            }

            using var doc = JsonDocument.Parse(body);
            doc.RootElement.TryGetProperty("token", out _).Should().BeFalse("a resposta não deve conter token");
        }

        [StepDefinition(@"eu uso o token recebido para acessar o endpoint ""([^""]*)""")]
        public async Task WhenEuUsoOTokenRecebidoParaAcessarOEndpoint(string endpoint)
        {
            _testContext.Token.Should().NotBeNullOrEmpty("deve existir um token para acessar o endpoint protegido");

            _testContext.Response = await GetApiClient().GetMeAsync(_testContext.Token!);
            _testContext.ResponseBody = await _testContext.Response.Content.ReadAsStringAsync();
        }

        [Then(@"a resposta do endpoint protegido deve ter status code (\d+)")]
        public void ThenARespostaDoEndpointProtegidoDeveTerStatusCode(int expectedStatusCode)
        {
            ThenARespostaDeveTerStatusCode(expectedStatusCode);
        }

        [Then(@"a resposta deve conter os dados do usuário autenticado")]
        [Then(@"a resposta do endpoint protegido deve conter os dados do usuário autenticado")]
        public void ThenARespostaDeveConterOsDadosDoUsuarioAutenticado()
        {
            _testContext.ResponseBody.Should().NotBeNullOrEmpty();

            var responseJson = JsonDocument.Parse(_testContext.ResponseBody!);
            var root = responseJson.RootElement;

            if (root.TryGetProperty("user", out var userProperty))
            {
                // Estrutura de resposta do login
                var user = userProperty;
                user.TryGetProperty("id", out _).Should().BeTrue("o usuário deve ter um ID");
                user.TryGetProperty("email", out _).Should().BeTrue("o usuário deve ter um email");
                user.TryGetProperty("username", out _).Should().BeTrue("o usuário deve ter um username");
            }
            else
            {
                // Estrutura direta do endpoint /me
                root.TryGetProperty("id", out _).Should().BeTrue("o usuário deve ter um ID");
                root.TryGetProperty("email", out _).Should().BeTrue("o usuário deve ter um email");
                root.TryGetProperty("username", out _).Should().BeTrue("o usuário deve ter um username");
            }
        }

        [Then(@"o token deve ser válido para acessar recursos protegidos")]
        public async Task ThenOTokenDeveSerValidoParaAcessarRecursosProtegidos()
        {
            _testContext.Token.Should().NotBeNullOrEmpty("deve existir um token");

            var response = await GetApiClient().GetMeAsync(_testContext.Token!);
            response.StatusCode.Should().Be(HttpStatusCode.OK, 
                "o token deve permitir acesso ao endpoint protegido /me");
        }
    }
}

