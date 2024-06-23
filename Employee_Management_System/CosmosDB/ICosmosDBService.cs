using Employee_Management_System.DTO;
using Employee_Management_System.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee_Management_System.CosmosDB
{
    public interface ICosmosDBService
    {
      
        Task<EmployeeBasicEntity> AddEmployeeBasicDetails(EmployeeBasicEntity employeeEntity);
        Task<List<EmployeeBasicEntity>> GetAllEmployeeBasicDetails();
        Task<EmployeeBasicEntity> GetEmployeeBasicDetailsByUId(string UId);
        Task ReplaceAsync(string UId, EmployeeBasicEntity existingUser);

        Task<EmployeeBasicDTO> AddEmployeeBasicDetails(EmployeeBasicDTO employeeBasicDetailsDTO);


        Task<EmployeeAdditionalEntity> AddEmployeeAdditionalDetails(EmployeeAdditionalEntity additionalDetailsEntity);
        Task<List<EmployeeAdditionalEntity>> GetAllEmployeeAdditionalDetails();
        Task<EmployeeAdditionalEntity> GetEmployeeAdditionalDetailsByBasicDetailsUId(string basicDetailsUId);
        Task ReplaceAdditionalDetailsAsync(string UId, EmployeeAdditionalEntity additionalDetailsEntity);
    }
}
