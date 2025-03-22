using Newtonsoft.Json;

namespace StudentMarksheetUploadApi.Model
{
    public class StudentMarkSheet
    {
        public required List<Student> Students { get; set; } = new List<Student>();
    }

    public class Student
    {
        [JsonProperty("rollNumber")]
        public required string RollNumber { get; set; }

        [JsonProperty("name")]
        public required string Name { get; set; }

        [JsonProperty("gender")]
        public required string Gender { get; set; }

        [JsonProperty("marks")]
        public required Marks Marks { get; set; }

        [JsonProperty("totalMarks")]
        public int TotalMarks { get; set; }

        [JsonProperty("status")]
        public required string Status { get; set; }

        [JsonProperty("id")]
        public required string Id { get; set; }
    }

    public class Marks
    {
        public int Maths { get; set; }
        public int Physics { get; set; }
        public int Chemistry { get; set; }
        public int SocialStudies { get; set; }
        public int SecondLanguage { get; set; }
    }
}
