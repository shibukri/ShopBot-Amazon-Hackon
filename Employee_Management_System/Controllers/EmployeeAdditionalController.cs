using AutoMapper;
using Employee_Management_System.DTO;
using Employee_Management_System.Entities;
using Employee_Management_System.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeeAdditionalController : ControllerBase
{
    private readonly IEmployeeAdditionalService _employeeAdditionalDetailsService;
    private readonly IMapper _mapper;

    public EmployeeAdditionalController(IEmployeeAdditionalService employeeAdditionalDetailsService, IMapper mapper)
    {
        _employeeAdditionalDetailsService = employeeAdditionalDetailsService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<EmployeeAdditionalDTO> AddEmployeeAdditionalDetails(EmployeeAdditionalDTO additionalDetailsDTO)
    {
        var entity = _mapper.Map<EmployeeAdditionalEntity>(additionalDetailsDTO);
        var response = await _employeeAdditionalDetailsService.AddEmployeeAdditionalDetails(entity);
        return _mapper.Map<EmployeeAdditionalDTO>(response);
    }

    [HttpGet]
    public async Task<List<EmployeeAdditionalDTO>> GetAllEmployeeAdditionalDetails()
    {
        var response = await _employeeAdditionalDetailsService.GetAllEmployeeAdditionalDetails();
        return _mapper.Map<List<EmployeeAdditionalDTO>>(response);
    }

    [HttpPost]
    public async Task<EmployeeAdditionalDTO> GetEmployeeAdditionalDetailsByBasicDetailsUId(string basicDetailsUId)
    {
        return await _employeeAdditionalDetailsService.GetEmployeeAdditionalDetailsByBasicDetailsUId(basicDetailsUId);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAdditionalDetailsByBasicDetailsUId(string basicDetailsUId, [FromBody] EmployeeAdditionalDTO additionalDetailsDTO)
    {
        var updatedDetails = await _employeeAdditionalDetailsService.UpdateAdditionalDetailsByBasicDetailsUId(basicDetailsUId, additionalDetailsDTO);
        return Ok(updatedDetails);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAdditionalDetailsByBasicDetailsUId(string basicDetailsUId)
    {
        var response = await _employeeAdditionalDetailsService.DeleteAdditionalDetailsByBasicDetailsUId(basicDetailsUId);
        return Ok("Data Deleted");
    }
}
