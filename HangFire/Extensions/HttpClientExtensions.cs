namespace HangFire.Extensions
{
    public class HttpClientExtensions
    {
        public static async Task<string> PostAsJson(string url, string data, string token = null)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if(token != null)
            {
                request.Headers.Add("authorization", token);
            }
            var content = new StringContent(data, null, "application/json");
            request.Content = content;
            var size = request.Content.Headers.ContentLength;
            var response = await client.SendAsync(request);
            string respData;

            if (response.IsSuccessStatusCode)
            {
                respData = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            return respData;
        }

        public static async Task<string> PutAsJson(string url, string data, string token = null)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            if (token != null)
            {
                request.Headers.Add("authorization", token);
            }
            var content = new StringContent(data, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            string respData;

            if (response.IsSuccessStatusCode)
            {
                respData = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            return respData;
        }

        public static async Task<string> GetAsJson(string url, string token = null)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (token != null)
            {
                request.Headers.Add("authorization", token);
            }
            //var content = new StringContent(data, null, "application/json");
            //request.Content = content;
            var response = await client.SendAsync(request);
            string respData;

            if (response.IsSuccessStatusCode)
            {
                respData = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            return respData;
        }
    }
}
