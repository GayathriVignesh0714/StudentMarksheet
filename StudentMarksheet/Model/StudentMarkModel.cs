using System.ComponentModel;

namespace StudentMarksheet.Model
{
    public class StudentMarkModel : StudentMarksEntity
    {
        public List<Marks> MarkList { get; set; } = new List<Marks>();
        public bool IsPassMark { get; set; } = false;
    }

    public enum ExamResult
    {
        [Description("ALL")]
        ALL,
        [Description("PASS")]
        PASS,
        [Description("FAIL")]
        FAIL
    }
}
