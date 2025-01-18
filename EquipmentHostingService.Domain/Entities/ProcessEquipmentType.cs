using System.ComponentModel.DataAnnotations;

namespace EquipmentHostingService.Domain.Entities;

public class ProcessEquipmentType
{
    [Key]
    public string Code { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    public double Area { get; set; }

    // Relationships
    public ICollection<FacilityEquipmentType> FacilityEquipmentTypes { get; set; } = new List<FacilityEquipmentType>();

    public ICollection<EquipmentPlacementContract> EquipmentPlacementContracts { get; set; } = new List<EquipmentPlacementContract>();
}
