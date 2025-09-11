using ProSolution.BL.DTOs.Characteristics;
using ProSolution.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.DTOs.ProductFeatureKeys
{
    public record ProductFeatureKeysCreateDto
    {
        public string CategoryId { get; set; } = null!;
        public List<string> FeatureOptionIds { get; set; } = new();
    }

    public record ProductFeatureKeysUpdateDto : ProductFeatureKeysCreateDto
    {
        public string Id { get; set; } = null!;
    }

    public record ProductFeatureKeysGetDto
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public List<FeatureOptionIncludeDto> FeatureOptions { get; set; }
    }

}
