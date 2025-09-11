using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.DTOs.User
{
    public record ChangeRoleDto
    {
        public string AppUserId { get; init; } = null!;
        public int Role { get; init; }
    }
}
