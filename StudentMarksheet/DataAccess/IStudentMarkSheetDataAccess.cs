using StudentMarksheet.Model;

namespace StudentMarksheet.DataAccess
{
    public interface IStudentMarkSheetDataAccess
    {
        Task InsertStudentMarkSheetAsync(List<StudentMarksEntity> studentMarks);
        Task<List<StudentMarksEntity>> FetchStudentMarkSheetAsync();
        Task<bool> UploadStudentMarksAsync(StudentMarkSheet students);
    }
}