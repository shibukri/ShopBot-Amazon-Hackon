using Employee_Management_System.DTO;
using Employee_Management_System.Entities;

public interface IEmployeeAdditionalService
{
    Task<EmployeeAdditionalEntity> AddEmployeeAdditionalDetails(EmployeeAdditionalEntity additionalDetailsEntity);
    Task<List<EmployeeAdditionalEntity>> GetAllEmployeeAdditionalDetails();
    Task<EmployeeAdditionalDTO> GetEmployeeAdditionalDetailsByBasicDetailsUId(string basicDetailsUId);
    Task<EmployeeAdditionalDTO> UpdateAdditionalDetailsByBasicDetailsUId(string basicDetailsUId, EmployeeAdditionalDTO additionalDetailsDTO);
    Task<EmployeeAdditionalDTO> DeleteAdditionalDetailsByBasicDetailsUId(string basicDetailsUId);
}
