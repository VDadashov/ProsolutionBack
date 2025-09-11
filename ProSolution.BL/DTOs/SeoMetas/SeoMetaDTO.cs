using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record SEOMetaDTO:BaseEntityDTO
{
    public string? MetaDescription { get; set; }
    public string? MetaTitle { get; set; }
     
    public string MetaTags { get; set; }
   
}
