using ProSolution.BL.DTOs.Commons;
namespace ProSolution.BL.DTOs;


public record SEOUrlDTO:BaseEntityDTO
{
    
    public string Url { get; set; }
    public string RedirectUrl { get; set; }
}
