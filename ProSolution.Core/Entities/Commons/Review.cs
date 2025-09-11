namespace ProSolution.Core.Entities.Commons
{

    public abstract class Review : BaseEntity
    {
        public string Text { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Checked { get; set; }
    }
}