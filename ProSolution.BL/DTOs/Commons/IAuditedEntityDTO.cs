using Microsoft.AspNetCore.Http;

namespace ProSolution.BL.DTOs.Commons
{
    public interface IAuditedEntityDTO
    {
        public IFormFile File { get; set; }
    }
}
