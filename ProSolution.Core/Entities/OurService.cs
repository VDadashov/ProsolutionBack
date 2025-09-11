using ProSolution.Core.Entities.Commons;

namespace ProSolution.Core.Entities
{
    public class OurService : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }      
        public string ImagePath { get; set; }
        
        public string ContentTitle { get; set; }
        public string ContentDescription { get; set; }

        public string ContentPath { get; set; }
       
    }
}
