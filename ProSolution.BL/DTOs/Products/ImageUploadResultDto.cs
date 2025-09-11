namespace ProSolution.BL.DTOs;

public record ImageUploadResultDto
{
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public string? AltText { get; set; }
}
