namespace StudentMarksheet.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class StudentMarkSheet
    {
        public List<StudentMarksEntity> Students { get; set; }
    }

    public class StudentMarksEntity
    {
        [Key] // Primary Key
        public string RollNumber { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        public int TotalMarks { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        public string Id { get; set; } = string.Empty;

        // Navigation property (One-to-One)
        public Marks Marks { get; set; } = new Marks();
    }

    public class Marks
    {
        [Key]
        [ForeignKey("Student")] // Foreign Key to Student Table
        public string RollNumber { get; set; } = string.Empty;

        public int Maths { get; set; }
        public int Physics { get; set; }
        public int Chemistry { get; set; }
        public int SocialStudies { get; set; }
        public int SecondLanguage { get; set; }

        // Navigation property (One-to-One)
        public StudentMarksEntity Student { get; set; } = null!;
    }
}
