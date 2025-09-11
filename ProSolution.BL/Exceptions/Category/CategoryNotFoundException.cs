namespace ProSolution.BL.Exceptions.Category
{
    public class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException() : base("Category is not found ") { }
        public CategoryNotFoundException(string message) : base(message) { }
    }
}
