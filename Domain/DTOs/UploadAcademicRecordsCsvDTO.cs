using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class UploadAcademicRecordsCsvDTO
{
    [Required]
    public IFormFile CsvFile { get; set; } = default!;

    [Required]
    public string GroupId { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string Term { get; set; } = default!;

    [Required]
    public AssessmentType AssessmentType { get; set; }
    public string? UploadingInstructorId { get; set; }
    public AcademicRecordStatus DefaultStatus { get; set; } = AcademicRecordStatus.Final;
}
public class ParsedAcademicRecordCsvRowDTO
{
    public string StudentId { get; set; } = default!;
    public int GradeValue { get; set; }
    public int OriginalRowNumber { get; set; }
}
