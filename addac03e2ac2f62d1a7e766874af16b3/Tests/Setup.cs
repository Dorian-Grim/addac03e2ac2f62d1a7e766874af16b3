using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests
{
    public class Init
    {
        internal HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }
    }
}
