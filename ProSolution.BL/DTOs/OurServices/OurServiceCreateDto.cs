using Microsoft.AspNetCore.Http;

namespace ProSolution.BL.DTOs;

public record OurServiceCreateDto 
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile Image{ get; set; }

    public string ContentTitle { get; set; }
    public string ContentDescription { get; set; }

    public IFormFile ContentImage { get; set; }
}