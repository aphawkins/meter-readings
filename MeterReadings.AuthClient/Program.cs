namespace ConsoleApp4
{
	using System;
	using System.Net.Http;
	using System.Net.Http.Json;
	using System.Text.Json.Serialization;
	using System.Threading.Tasks;
	using Newtonsoft.Json.Linq;
	using RestSharp;

	internal static class Program
    {
        private static async Task Main()
        {
            TokenRequest tokenRequest = new()
            {
                ClientId = "{ClientId}",
                ClientSecret = "{ClientSecret}",
                Audience = "https://aph-meter-readings.com",
                GrantType = "client_credentials"
            };

            using HttpClient httpClient = new();
            Console.WriteLine(await GetBearerTokenAsync(httpClient, tokenRequest));

            RestClient restClient = new("https://dev-70o00lzn.eu.auth0.com/oauth/token");
            Console.WriteLine(GetBearerToken(restClient, tokenRequest));
            Console.Read();
        }

        private static async Task<string> GetBearerTokenAsync(HttpClient client, TokenRequest tokenRequest)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<TokenRequest>("https://dev-70o00lzn.eu.auth0.com/oauth/token", tokenRequest);
            response.EnsureSuccessStatusCode();
            TokenResponse tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
            return tokenResponse.AccessToken;
        }

        private static string GetBearerToken(RestClient restClient, TokenRequest tokenRequest)
        {
            RestRequest request = new(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", $"{{\"client_id\":\"{tokenRequest.ClientId}\",\"client_secret\":\"{tokenRequest.ClientSecret}\",\"audience\":\"{tokenRequest.Audience}\",\"grant_type\":\"{tokenRequest.GrantType}\"}}", ParameterType.RequestBody);
            IRestResponse response = restClient.Execute(request);
            JObject content = JObject.Parse(response.Content);
            return (string)content["access_token"];
        }
    }

    internal class TokenRequest
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("audience")]
        public string Audience { get; set; }

        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }
    }

    internal class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}
