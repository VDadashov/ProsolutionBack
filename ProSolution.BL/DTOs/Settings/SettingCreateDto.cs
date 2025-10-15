namespace ProSolution.BL.DTOs
{
    public record SettingCreateDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}