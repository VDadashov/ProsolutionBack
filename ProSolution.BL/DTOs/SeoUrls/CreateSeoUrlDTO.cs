namespace ProSolution.BL.DTOs;


public record CreateSeoUrlDTO
{
    public string Url { get; set; }
    public string RedirectUrl { get; set; }
}
