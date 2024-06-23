using AutoMapper;
using Employee_Management_System.DTO;
using Employee_Management_System.Entities;
using Employee_Management_System.Interfaces;
using Employee_Management_System.ServiceFilters;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Threading.Tasks;

namespace Employee_Management_System.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeBasicController : ControllerBase
    {
        private readonly IEmployeeBasicService _employeeBasicDetailsService;
        private readonly IEmployeeAdditionalService _employeeAdditionalDetailsService;
        private readonly IMapper _mapper;

        public EmployeeBasicController(IEmployeeBasicService employeeBasicDetailsService, IMapper mapper)
        {
            _employeeBasicDetailsService = employeeBasicDetailsService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<EmployeeBasicDTO> AddEmployeeBasicDetails(EmployeeBasicDTO employeeBasicDetailsDTO)
        {
            var model = _mapper.Map<EmployeeBasicEntity>(employeeBasicDetailsDTO);
            var response = await _employeeBasicDetailsService.AddEmployeeBasicDetails(model);
            return _mapper.Map<EmployeeBasicDTO>(response);
        }

        [HttpPost]
        public async Task<EmployeeBasicDTO> GetEmployeeBasicDetailsByUId(string UId)
        {
            return await _employeeBasicDetailsService.GetEmployeeBasicDetailsByUId(UId);
        }

        [HttpGet]
        public async Task<List<EmployeeBasicDTO>> GetAllEmployeeBasicDetails()

        {
            var response = await _employeeBasicDetailsService.GetAllEmployeeBasicDetails();
            return _mapper.Map<List<EmployeeBasicDTO>>(response);
        }

        
        [HttpPost]
        public async Task<IActionResult> UpdateUserDetailsByUId(string UId, EmployeeBasicDTO employeeBasicDetailsDTO)
        {
            var updatedUser = await _employeeBasicDetailsService.UpdateUserDetailsByUId(UId, employeeBasicDetailsDTO);
            return Ok(updatedUser);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserByUId(string UId)
        {
            var response = await _employeeBasicDetailsService.DeleteUserByUId(UId);
            return Ok("User Deleted");
        }


        [HttpGet]
         public async Task<IActionResult> GetAllEmployeeBasicDetailsByRole(string role)
        {
            var response = await _employeeBasicDetailsService.GetAllEmployeeBasicDetailsByRole(role);
            return Ok(response);
        }

        [HttpPost]
        [ServiceFilter(typeof(BuildEmployeeFilter)) ]
        public async Task<EmployeeFilterCriteria> GetAllEmployeeBasicDetailsByPagination(EmployeeFilterCriteria employeeFilterCriteria)
        {
            var response =  await _employeeBasicDetailsService.GetAllEmployeeBasicDetailsByPagination(employeeFilterCriteria);
            return response;
        }
        [HttpPost]
        public async Task<IActionResult> AddstudentByMakePostRequest(EmployeeBasicDTO employeeBasicDTO)
        {
            var response = await _employeeBasicDetailsService.AddstudentByMakePostRequest(employeeBasicDTO);
            return Ok(response);
        }

        [HttpGet]
        public async Task<List<EmployeeBasicDTO>> GetAllStudentByMakeGetRequest()
        {
            var respnse = await _employeeBasicDetailsService.GetAllStudentByMakeGetRequest();
            return respnse;
        }

    }
}

