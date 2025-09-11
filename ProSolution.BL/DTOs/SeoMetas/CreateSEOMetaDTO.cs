namespace ProSolution.BL.DTOs;

public record CreateSEOMetaDTO
{
    public string? MetaDescription { get; set; }
    public string? MetaTitle { get; set; }
    
    public string MetaTags { get; set; }

}
