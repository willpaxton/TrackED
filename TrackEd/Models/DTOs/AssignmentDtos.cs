namespace TrackEd.Models.DTOs
{
    public record AssignmentReadDto(
        int AssignmentID,
        int StudentEnumber,
        int ProfessorEnumber,
        int LocationID,
        DateOnly Date,
        int StartTime,
        int EndTime,
        // Optional: denormalized display fields to help the UI
        string? ProfessorName = null,
        string? LocationName = null
    );

    public record AssignmentCreateDto(
        int StudentEnumber,
        int ProfessorEnumber,
        int LocationID,
        DateOnly Date,
        int StartTime,
        int EndTime
    );

    public record AssignmentUpdateDto(
        int StudentEnumber,
        int ProfessorEnumber,
        int LocationID,
        DateOnly Date,
        int StartTime,
        int EndTime
    );
}