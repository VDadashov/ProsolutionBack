using ProSolution.BL.DTOs.Characteristics;
using ProSolution.BL.DTOs.FeatureOPtions;

namespace ProSolution.BL.DTOs;

public record PaginationDto<T>
{
    public string? SlugPath { get; set; }
    public double? MaxPrice { get; set; }
    public double? MaxAvailablePrice { get; set; }
    public double? MinPrice { get; set; }
    public double? MinAvailablePrice { get; set; }
    public int Take { get; init; }
    public bool IsDiscount { get; init; }
    public string? Author { get; init; }
    public ICollection<string>? FeatureIds { get; set; }

    public ICollection<FeatureOptionGetDto>? CategoryFeatures { get; set; }

    public int Order { get; init; }
    public double Count { get; init; }
    public string? Search { get; init; }
    public int CurrentPage { get; init; }
    public double TotalPage { get; init; }
    public ICollection<T>? Items { get; init; }
    public T? Item { get; init; }
}
