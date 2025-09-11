namespace ProSolution.Core.Entities;

public class Category : BaseEntity
{
    public string Title { get; set; }
    public string? Slug { get; set; }
    public int? Index { get; set; }

    public string? ParentId { get; set; }
    public Category? Parent { get; set; }
    public ICollection<Category>? Children { get; set; }

    public ICollection<CategoryProduct>? CategoryProducts { get; set; }
    public ICollection<Blog>? Blogs { get; set; }

    public ICollection<ProductFeatureKeys>? ProductFeatureKeys { get; set; } = new List<ProductFeatureKeys>();
}