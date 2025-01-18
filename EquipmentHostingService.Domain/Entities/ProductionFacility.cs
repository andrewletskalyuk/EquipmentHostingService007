using System.ComponentModel.DataAnnotations;

namespace EquipmentHostingService.Domain.Entities;

public class ProductionFacility
{
    [Key]
    public string Code { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    [Range(1.0, Double.MaxValue, ErrorMessage = "Standard area must be greater than zero.")]
    public double StandardAreaForEquipment { get; set; }

    public ICollection<EquipmentPlacementContract> EquipmentPlacementContracts { get; set; } = new List<EquipmentPlacementContract>();

    public ICollection<FacilityEquipmentType> FacilityEquipmentTypes { get; set; } = new List<FacilityEquipmentType>();
}
