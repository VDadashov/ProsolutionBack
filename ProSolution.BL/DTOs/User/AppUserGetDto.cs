using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.DTOs.User
{
    public record AppUserGetDto
    {
        public string Id { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Surname { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string? Role { get; set; }

        public string? Slug { get; set; }   
        public bool isAutuhenticated { get; set; } = false;


    }
}
