using System.ComponentModel.DataAnnotations;

namespace EquipmentHostingService.Domain.Entities;

public class EquipmentPlacementContract
{
    public int Id { get; set; }

    [Required]
    public string ProductionFacilityCode { get; set; }

    public ProductionFacility ProductionFacility { get; set; }

    [Required]
    public required string ProcessEquipmentTypeCode { get; set; }

    public ProcessEquipmentType ProcessEquipmentType { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The number of units must be at least 1.")]
    public int NumberOfUnits { get; set; }
}
