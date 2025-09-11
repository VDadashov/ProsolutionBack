using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.Core.Entities
{
    public class ProductFeatureKeys : BaseEntity
    {
        public Category Category { get; set; }
        public string CategoryId { get; set; }



        public ICollection<FeatureOption> FeatureOptions { get; set; } = new List<FeatureOption>();
    }
}
