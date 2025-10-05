using System.Net;

namespace Tests
{
    public class Tests : Init
    {
        [Test]
        public async Task TestGetCity()
        {
            var r = await _client.GetAsync("/api/getcity/0");
            Assert.That(r.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}