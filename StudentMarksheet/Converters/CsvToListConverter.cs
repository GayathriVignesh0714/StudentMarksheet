using StudentMarksheet.Model;
using System.IO;

public class CsvToListConverter
{
    public List<StudentMarksEntity> ConvertCsvToList(string filePath)
    {
        try
        {
            var lines = File.ReadAllLines(filePath);
            if (lines.Length == 0) return new List<StudentMarksEntity>(); // Return empty list if no data

            var headers = lines[0].Split(','); // Read CSV headers
            var students = new List<StudentMarksEntity>();

            foreach (var line in lines.Skip(1)) // Skip the header
            {
                var values = line.Split(',');

                var student = new StudentMarksEntity
                {
                    RollNumber = values.Length > 0 ? values[0].Trim() : string.Empty,
                    Name = values.Length > 1 ? values[1].Trim() : string.Empty,
                    Gender = values.Length > 2 ? values[2].Trim() : string.Empty,
                    TotalMarks = values.Length > 3 && int.TryParse(values[3].Trim(), out int totalMarks) ? totalMarks : 0,
                    Status = values.Length > 4 ? values[4].Trim() : "N/A",
                    Id = values.Length > 0 ? values[0].Trim() : string.Empty,
                    Marks = new Marks()
                    {
                        Maths = values.Length > 5 && int.TryParse(values[5].Trim(), out int maths) ? maths : 0,
                        Physics = values.Length > 6 && int.TryParse(values[6].Trim(), out int physics) ? physics : 0,
                        Chemistry = values.Length > 7 && int.TryParse(values[7].Trim(), out int chemistry) ? chemistry : 0,
                        SocialStudies = values.Length > 8 && int.TryParse(values[8].Trim(), out int social) ? social : 0,
                        SecondLanguage = values.Length > 9 && int.TryParse(values[9].Trim(), out int secondLang) ? secondLang : 0
                    }
                };

                students.Add(student);
            }

            return students;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error converting CSV to list: {ex.Message}");
            return new List<StudentMarksEntity>(); // Corrected return type
        }
    }
}
