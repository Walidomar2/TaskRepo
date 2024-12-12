
using Newtonsoft.Json;

namespace LoggingSystem.API.Services
{
    public class S3LogStorage : ILogStorage
    {

        private readonly HttpClient _httpClient;
        private readonly string _bucketUrl;

        public S3LogStorage(HttpClient httpClient, string bucketUrl)
        {
            _httpClient = httpClient;
            _bucketUrl = bucketUrl;
        }


        public Task<List<Log?>> GetLogsAsync(string? service, string? level, DateTime? start_time, DateTime? end_time)
        {
            throw new NotImplementedException();
        }

        public async Task SaveLogAsync(Log logEntry)
        {
            var fileUrl = $"{_bucketUrl}/{logEntry.Id}.json";
            var content = new StringContent(JsonConvert.SerializeObject(logEntry));

            var response = await _httpClient.PutAsync(fileUrl, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to save log to S3. Status code: {response.StatusCode}");
        }
    }
}
