using Employee_Management_System.DTO;
using Employee_Management_System.Entities;

namespace Employee_Management_System.Interfaces
{
    public interface IEmployeeBasicService
    {
        Task<EmployeeBasicEntity> AddEmployeeBasicDetails(EmployeeBasicEntity employeeEntity);
        Task<List<EmployeeBasicEntity>> GetAllEmployeeBasicDetails();
        Task<EmployeeBasicDTO> GetEmployeeBasicDetailsByUId(string uId);

        Task<EmployeeBasicDTO> UpdateUserDetailsByUId(string UId, EmployeeBasicDTO employeeBasicDetailsDTO);

        Task<EmployeeBasicDTO> DeleteUserByUId(string UId);

        Task<List<EmployeeBasicEntity>> GetAllEmployeeBasicDetailsByRole(string role);  

        Task<EmployeeBasicDTO> AddEmployeeBasicDetails(EmployeeBasicDTO employee);
        Task<EmployeeFilterCriteria> GetAllEmployeeBasicDetailsByPagination(EmployeeFilterCriteria employeeFilterCriteria);
        Task<List<EmployeeBasicDTO>> GetAllStudentByMakeGetRequest();
        Task<EmployeeBasicDTO> AddstudentByMakePostRequest(EmployeeBasicDTO employeeBasicDTO);
    }
}
