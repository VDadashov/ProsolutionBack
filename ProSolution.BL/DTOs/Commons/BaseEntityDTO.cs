namespace ProSolution.BL.DTOs.Commons
{
    public abstract record BaseEntityDTO : AuditedEntityDTO
    {
        public string? Id { get; set; }
    }
}
