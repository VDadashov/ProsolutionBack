using ProSolution.BL.DTOs.Commons;
namespace ProSolution.BL.DTOs;


public record SEODTO:BaseEntityDTO
{
    public string MetaDescription { get; set; }
    public string MetaTitle { get; set; }
    public string AltText { get; set; }
    public string AnchorText { get; set; }
    public string MetaTags { get; set; }
    public string Url { get; set; }
    public string RedirectUrl { get; set; }
}
