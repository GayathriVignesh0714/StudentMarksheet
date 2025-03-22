using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StudentMarksheet.Model;
using System.Net.Http;
using System.Text;

namespace StudentMarksheet.ApiService
{
    public class StudentMarksheetApiService : IStudentMarksheetApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StudentMarksheetApiService> _logger;
        private readonly string _apiUrl;
        private readonly string _postApiUrl;

        public StudentMarksheetApiService(HttpClient httpClient, IOptions<AppSettings> options, ILogger<StudentMarksheetApiService> logger)
        {
            if (string.IsNullOrWhiteSpace(options?.Value?.StudentMarkSheetUrl))
            {
                logger.LogError("API URL is missing in configuration.");
                throw new InvalidOperationException("Missing API URL in configuration.");
            }

            if (string.IsNullOrWhiteSpace(options?.Value?.PostStudentMarkSheetUrl))
            {
                logger.LogError("Post API URL is missing in configuration.");
                throw new InvalidOperationException("Missing Post API URL in configuration.");
            }

            _apiUrl = options.Value.StudentMarkSheetUrl;
            _postApiUrl = options.Value.PostStudentMarkSheetUrl;
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Fetches student marksheet from the API using Newtonsoft.Json
        /// </summary>
        public async Task<List<StudentMarksEntity>> FetchStudentMarkSheetAsync()
        {
            try
            {
                _logger.LogInformation("Fetching student marksheet from API...");

                using HttpResponseMessage response = await _httpClient.GetAsync(_apiUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize using Newtonsoft.Json
                var students = JsonConvert.DeserializeObject<List<StudentMarksEntity>>(jsonResponse) ?? new List<StudentMarksEntity>();

                students = students.Where(s => !string.IsNullOrWhiteSpace(s.RollNumber)).ToList();

                _logger.LogInformation("No. of StudentsMarkSheet Fetched: {Count}", students.Count);
                return students;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error while fetching StudentMarkSheet");
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Error deserializing StudentMarkSheet JSON response");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching StudentMarkSheet");
            }

            return new List<StudentMarksEntity>(); // Return empty on error
        }

        /// <summary>
        /// Posts student marksheet data to the API using Newtonsoft.Json
        /// </summary>
        public async Task<bool> PostStudentMarkSheetAsync(StudentMarkSheet newStudents)
        {
            try
            {
                if (newStudents.Students == null || newStudents.Students.Count == 0)
                {
                    _logger.LogWarning("No student marks to upload.");
                    return false;
                }

                //  Convert to JSON using Newtonsoft.Json to ensure correct structure
                string jsonData = JsonConvert.SerializeObject(newStudents, Formatting.Indented);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                //  Send ONLY new students in the request
                using HttpResponseMessage response = await _httpClient.PostAsync(_postApiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to upload data. Status: {StatusCode}, Response: {Response}", response.StatusCode, errorMessage);
                    return false;
                }

                _logger.LogInformation("Successfully posted {Count} student marksheet details.", newStudents.Students?.Count);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while posting data: {ex.Message}");
                return false;
            }
        }
    }
}
