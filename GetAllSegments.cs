namespace GetAllSegments {
    class Program {
        private static string User = "YOUR_USER";
        private static string Password = "YOUR_PASSWORD";
        private static string? Token;
        private static readonly HttpClient client = new HttpClient();

        private static async Task<string> getToken() {
            client.BaseAddress = new Uri("https://api.remarkets.primary.com.ar");
            var request = new HttpRequestMessage(HttpMethod.Post, "/auth/getToken");

            var formData = new List<KeyValuePair<string, string>>();

            request.Content = new FormUrlEncodedContent(formData);

            request.Content.Headers.Clear();
            request.Content.Headers.Add("X-Username", User);
            request.Content.Headers.Add("X-Password", Password);

            var response = await client.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();
            var values = response.Headers.GetValues("X-Auth-Token");
            foreach (var value in values)
            {
                Token = value.ToString();
            }
            return Token;
        }
        
        static async Task Main()
        {
            try	
            {
                await getToken();
                Console.WriteLine($"Token: {Token}");

                HttpResponseMessage response = await client.GetAsync("https://api.remarkets.primary.com.ar/rest/segment/all");
                response.Headers.Add("X-Auth-Token", Token);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }
        }
    }
}