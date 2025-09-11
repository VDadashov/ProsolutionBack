using Microsoft.AspNetCore.Http;

namespace ProSolution.BL.DTOs;

public record BlogUpdateDto
{

    public string Title { get; set; }
   
    
  
    public IFormFile? Image { get; set; }
    public string Description { get; set; }



    public string CategoryId { get; set; }
}
