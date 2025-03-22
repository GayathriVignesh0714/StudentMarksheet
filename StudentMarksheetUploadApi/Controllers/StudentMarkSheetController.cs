using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentMarksheetUploadApi.Model;

[Route("api/[controller]")]
[ApiController]
public class StudentMarkSheetController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StudentMarkSheetController> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _filePath;
    private readonly string _studentMarkSheetUrl;

    public StudentMarkSheetController(
        IConfiguration configuration,
        HttpClient httpClient,
        ILogger<StudentMarkSheetController> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        _filePath = _configuration["UploadSettings:FilePath"]
                    ?? throw new ArgumentException("FilePath is missing in configuration", nameof(_filePath));
        _studentMarkSheetUrl = _configuration["UploadSettings:StudentMarkSheetUrl"]
                               ?? throw new ArgumentException("StudentMarkSheetUrl is missing in configuration", nameof(_studentMarkSheetUrl));
    }

    /// <summary>
    /// Fetches student marksheet data from the mock API.
    /// </summary>
    [HttpGet("fetch")]
    public async Task<IActionResult> FetchStudentMarkSheet()
    {
        _logger.LogInformation("Fetching marksheet from {Url}", _studentMarkSheetUrl);

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            using HttpResponseMessage response = await _httpClient.GetAsync(_studentMarkSheetUrl, cts.Token);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to fetch data. Status: {StatusCode}", response.StatusCode);
                return StatusCode((int)response.StatusCode, new { message = "Failed to fetch marksheet", status = response.StatusCode });
            }

            string jsonData = await response.Content.ReadAsStringAsync(cts.Token);
            if (string.IsNullOrWhiteSpace(jsonData))
            {
                throw new Exception("JSON data is empty or null.");
            }

            var studentMarkSheet = JsonConvert.DeserializeObject<List<Student>>(jsonData)
                   ?.Where(s => !string.IsNullOrWhiteSpace(s.RollNumber)) // Filter out empty records
                   .ToList() ?? new List<Student>();

            _logger.LogInformation($"Successfully deserialized {studentMarkSheet.Count} students.");

            _logger.LogInformation("Marksheet fetched successfully.");
            return Content(jsonData, "application/json");
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Request timed out.");
            return StatusCode(408, new { message = "Request timeout. The API took too long to respond." });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "fetching student marksheet");
        }
    }

    /// <summary>
    /// Uploads a JSON file to the mock API using HTTP POST.
    /// </summary>
    [HttpPost("upload")]
    public async Task<IActionResult> UploadStudentMarkSheet([FromBody] StudentMarkSheet studentMarkSheet)
    {
        if (studentMarkSheet?.Students == null || studentMarkSheet.Students.Count == 0)
        {
            _logger.LogWarning("Received an empty or null student mark sheet list.");
            return BadRequest(new { message = "Invalid or empty student mark sheet data." });
        }

        try
        {
            _logger.LogInformation("Fetching existing student data from {Url}.", _studentMarkSheetUrl);

            // Step 1: Fetch Existing Students
            HttpResponseMessage getResponse = await _httpClient.GetAsync(_studentMarkSheetUrl);
            string existingDataJson = await getResponse.Content.ReadAsStringAsync();

            List<Student> existingStudents = JsonConvert.DeserializeObject<List<Student>>(existingDataJson) ?? new List<Student>();

            int addedCount = 0;

            // Step 2: Process Each New Student
            foreach (var newStudent in studentMarkSheet.Students)
            {
                var existingStudent = existingStudents.FirstOrDefault(s => s.RollNumber == newStudent.RollNumber);

                if (existingStudent == null)
                {
                    // New student, add using POST
                    HttpResponseMessage postResponse = await _httpClient.PostAsJsonAsync(_studentMarkSheetUrl, newStudent);
                    if (postResponse.IsSuccessStatusCode) addedCount++;
                }
            }

            _logger.LogInformation("Successfully added {AddedCount} new students.", addedCount);
            return Ok(new { message = $"Student mark sheet updated successfully. Added:{addedCount}" });
        }
        catch (TaskCanceledException)
        {
            _logger.LogError("Request to {Url} timed out!", _studentMarkSheetUrl);
            return StatusCode(408, new { message = "Request timeout. The API took too long to respond." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating student mark sheet.");
            return StatusCode(500, new { message = "An error occurred while processing the request." });
        }
    }

    /// <summary>
    /// Handles exceptions by logging them and returning a standardized API response.
    /// </summary>
    private IActionResult HandleException(Exception ex, string action)
    {
        _logger.LogError(ex, "Error while {Action}: {Message}", action, ex.Message);
        return StatusCode(500, new { message = $"Error {action}", error = ex.Message });
    }
}
