namespace Clinical.Application.DTOS.Exam.Response
{
    public class GetExamByIdResponseDto
    {
        public int ExamId { get; set; }
        public string? Name { get; set; }
        public int? AnalysisId { get; set; }
    }
}
