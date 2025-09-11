using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record SettingGetDto : BaseEntityDTO
{
    public string Key { get; set; }
    public string Value { get; set; }
}
