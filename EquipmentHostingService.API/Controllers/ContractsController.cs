using EquipmentHostingService.Application.Dtos;
using EquipmentHostingService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentHostingService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContractsController : ControllerBase
{
    readonly IContractService _contractService;

	public ContractsController(IContractService contractService)
	{
		_contractService = contractService;
	}

    [HttpPost]
    public async Task<IActionResult> CreateContract([FromBody] EquipmentPlacementContractDto contractDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _contractService.CreateContract(contractDto);
        
        if (result.Success)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllContracts()
    {
        var contracts = await _contractService.GetAllContracts();
        return Ok(contracts);
    }
}
