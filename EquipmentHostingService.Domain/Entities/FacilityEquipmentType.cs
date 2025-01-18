namespace EquipmentHostingService.Domain.Entities;

public class FacilityEquipmentType
{
    public required string ProductionFacilityCode { get; set; }

    public ProductionFacility ProductionFacility { get; set; }

    public required string ProcessEquipmentTypeCode { get; set; }
    
    public ProcessEquipmentType ProcessEquipmentType { get; set; }
}
