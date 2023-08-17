using Microsoft.Extensions.Options;
using System.Text.Json;

namespace dotnet6
{
    public record Repo361AccessToken(string token_type, string access_token);
    public record Repo361CsvFile(string name, string url);
    public record LocalCsvFile(string name, string path);

    public class Repo361Api
    {
        public const string Repo361 = "Repo361";

        private readonly Repo361Settings _settings;
        private readonly IHttpClientFactory _clientFactory;

        public Repo361Api(IOptions<Repo361Settings> opts, IHttpClientFactory clientFactory)
        {
            _settings = opts.Value;
            _clientFactory = clientFactory;
        }

        public async IAsyncEnumerable<LocalCsvFile> DownloadAsync(DateTime dt)
        {
            var directory = "rawdata";
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            var files = await LoadCsvFilesAsync(dt);
            var client = _clientFactory.CreateClient();
            var dtFormatted = dt.ToString("yyyyMMdd");
            foreach (var file in files)
            {
                var fileName = $"{dtFormatted}_{file.name}.csv";
                var savePath = Path.Combine(directory, fileName);
                if (File.Exists(savePath)) File.Delete(savePath);
                await DownloadAsync(client, file, savePath);
                //Special for MySQL related location to sql execution, only '\\'
                yield return new LocalCsvFile(file.name, directory + @"\\" + fileName);
            }
        }

        private async Task<Repo361CsvFile[]> LoadCsvFilesAsync(DateTime dt)
        {
            var client = _clientFactory.CreateClient(Repo361);
            var token = await GetAccessTokenAsync(client, _settings);
            var files = await GetCsvFilesAsync(client, token, dt);
            return files;
        }

        private static async Task<Repo361CsvFile[]> GetCsvFilesAsync(HttpClient client, Repo361AccessToken token, DateTime dt)
        {
            var getUri = $"/rawdata/files/{dt.ToString("yyyyMMdd")}/csv";
            using var req = new HttpRequestMessage(HttpMethod.Get, getUri);
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token.token_type, token.access_token);
            using var res = await client.SendAsync(req);
            res.EnsureSuccessStatusCode();
            var resContent = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Repo361CsvFile[]>(resContent) ?? throw new ArgumentException("Cant deserialize data csv files");
        }

        private static async Task<Repo361AccessToken> GetAccessTokenAsync(HttpClient client, Repo361Settings settings)
        {
            ArgumentNullException.ThrowIfNull(settings.ClientId);
            ArgumentNullException.ThrowIfNull(settings.ClientSecret);

            var postUri = "/security/oauth2/token";
            var data = new Dictionary<string, string>();
            data.Add("client_id", settings.ClientId);
            data.Add("client_secret", settings.ClientSecret);
            data.Add("grant_type", "client_credentials");
            using var req = new HttpRequestMessage(HttpMethod.Post, postUri) { Content = new FormUrlEncodedContent(data) };
            using var res = await client.SendAsync(req);
            res.EnsureSuccessStatusCode();
            var resContent = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Repo361AccessToken>(resContent) ?? throw new ArgumentException("Cant deserialize data access token");
        }

        private static async Task DownloadAsync(HttpClient client, Repo361CsvFile file, string savePath)
        {
            var response = await client.GetAsync(file.url);
            if (response.IsSuccessStatusCode)
            {
                using (var contentStream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = System.IO.File.Create(savePath))
                {
                    await contentStream.CopyToAsync(fileStream);
                }
            }
            else
            {
                Console.WriteLine($"Failed to download file. Status code: {response.StatusCode}");
            }
        }

    }
}
