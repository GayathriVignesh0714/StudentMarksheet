using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentMarksheet.ApiService;
using StudentMarksheet.Model;

namespace StudentMarksheet.DataAccess
{
    public class StudentMarkSheetDataAccess : IStudentMarkSheetDataAccess
    {
        private readonly AppDBContext _dbContext;
        private readonly IStudentMarksheetApiService _apiService;
        private readonly ILogger<StudentMarkSheetDataAccess> _logger;

        public StudentMarkSheetDataAccess(AppDBContext dbContext, IStudentMarksheetApiService apiService, ILogger<StudentMarkSheetDataAccess> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Fetches student marksheet from API.
        /// </summary>
        public async Task<List<StudentMarksEntity>> FetchStudentMarkSheetAsync()
        {
            try
            {
                _logger.LogInformation("Fetching student marksheet from API...");
                var studentMarks = await _apiService.FetchStudentMarkSheetAsync();

                if (studentMarks == null || studentMarks.Count == 0)
                {
                    _logger.LogWarning("No student marks found in API response.");
                    return new List<StudentMarksEntity>(); // Return empty list if no data
                }

                _logger.LogInformation($"{studentMarks.Count} student marks retrieved successfully.");

                // Fire-and-forget insert operation with exception handling
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await InsertStudentMarkSheetAsync(studentMarks);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error inserting student marksheet.");
                    }
                }).ConfigureAwait(false); // Avoids unnecessary blocking

                return studentMarks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching student marksheet from API.");
                return new List<StudentMarksEntity>(); // Return empty list on error
            }
        }

        /// <summary>
        /// Inserts fetched student marks into the database efficiently.
        /// </summary>
        public async Task InsertStudentMarkSheetAsync(List<StudentMarksEntity> studentMarks)
        {
            if (studentMarks == null || studentMarks.Count == 0)
            {
                _logger.LogWarning("No student marks to insert.");
                return;
            }

            try
            {
                _logger.LogInformation("Checking for existing student marks in the database...");

                // Get existing RollNumbers in one query (avoiding N+1 problem)
                var existingRollNumbers = await _dbContext.StudentMarks
                    .Select(s => s.RollNumber)
                    .ToHashSetAsync();

                // Filter only new records
                var newStudents = studentMarks
                    .Where(s => !existingRollNumbers.Contains(s.RollNumber))
                    .ToList();

                if (newStudents.Count > 0)
                {
                    _logger.LogInformation($"Inserting {newStudents.Count} new student records into database...");

                    await _dbContext.StudentMarks.AddRangeAsync(newStudents); // Bulk insert
                    await _dbContext.SaveChangesAsync();

                    _logger.LogInformation("Student marks inserted successfully.");
                }
                else
                {
                    _logger.LogInformation("No new student marks to insert.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting student marks into database.");
            }
        }

        public async Task<bool> UploadStudentMarksAsync(StudentMarkSheet newStudentMarks)
        {
            try
            {
                // Post Updated JSON to API
                _logger.LogInformation("Posting updated data to API...");
                return await _apiService.PostStudentMarkSheetAsync(newStudentMarks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error appending student marks.");
                return false;
            }
        }
    }
}
