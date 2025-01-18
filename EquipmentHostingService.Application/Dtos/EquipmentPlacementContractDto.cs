using System.ComponentModel.DataAnnotations;

namespace EquipmentHostingService.Application.Dtos;

//on the other hand we can use FluentValidator to dug deeper with validation!!!
public class EquipmentPlacementContractDto
{
    [Required(ErrorMessage = "Production facility code is required.")]
    [StringLength(100, ErrorMessage = "Production facility code must not exceed 100 characters.")]
    public string ProductionFacilityCode { get; set; }

    [Required(ErrorMessage = "Process equipment type code is required.")]
    [StringLength(100, ErrorMessage = "Process equipment type code must not exceed 100 characters.")]
    public string ProcessEquipmentTypeCode { get; set; }

    [Required(ErrorMessage = "Number of units is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Number of units must be at least 1.")]
    public int NumberOfUnits { get; set; }
}
