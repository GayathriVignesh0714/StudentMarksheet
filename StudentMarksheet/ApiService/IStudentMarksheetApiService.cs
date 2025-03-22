using StudentMarksheet.Model;

namespace StudentMarksheet.ApiService
{
    public interface IStudentMarksheetApiService
    {
        Task<List<StudentMarksEntity>> FetchStudentMarkSheetAsync();
        Task<bool> PostStudentMarkSheetAsync(StudentMarkSheet newStudents);
    }
}