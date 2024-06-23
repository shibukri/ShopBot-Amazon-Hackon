using AutoMapper;
using Employee_Management_System.Common;
using Employee_Management_System.CosmosDB;
using Employee_Management_System.DTO;
using Employee_Management_System.Entities;
using Employee_Management_System.Interfaces;
using Microsoft.Azure.Cosmos.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Employee_Management_System.Services
{
    public class EmployeeBasicService : IEmployeeBasicService
    {
        private readonly ICosmosDBService _cosmosDBService;
        private readonly IMapper _mapper;

        public EmployeeBasicService(ICosmosDBService cosmosDBService, IMapper mapper)
        {
            _cosmosDBService = cosmosDBService;
            _mapper = mapper;
        }

        public async Task<EmployeeBasicEntity> AddEmployeeBasicDetails(EmployeeBasicEntity employeeEntity)
        {
            employeeEntity.Initialize(true, "Employee", "Sudh", "Sudh");
            employeeEntity = await _cosmosDBService.AddEmployeeBasicDetails(employeeEntity);
            return employeeEntity;
        }

        public async Task<List<EmployeeBasicEntity>> GetAllEmployeeBasicDetails()
        {
            return await _cosmosDBService.GetAllEmployeeBasicDetails();
        }



        public async Task<EmployeeBasicDTO> GetEmployeeBasicDetailsByUId(string UId)
        {
            var response = await _cosmosDBService.GetEmployeeBasicDetailsByUId(UId);
            return _mapper.Map<EmployeeBasicDTO>(response);

        }

        public async Task<EmployeeBasicDTO> UpdateUserDetailsByUId(string UId, EmployeeBasicDTO employeeBasicDetailsDTO)
        {
            var existingUser = await _cosmosDBService.GetEmployeeBasicDetailsByUId(UId);
            if (existingUser == null)
            {
                return null; 
            }

            existingUser.Active = false;
            existingUser.Archieved = true;

            await _cosmosDBService.ReplaceAsync(UId, existingUser);

            existingUser.Initialize(false, "Employee", "Sudh", "Sudh");
            _mapper.Map(employeeBasicDetailsDTO, existingUser);

            existingUser = await _cosmosDBService.AddEmployeeBasicDetails(existingUser);
            return _mapper.Map<EmployeeBasicDTO>(existingUser);
        }

        public async Task<EmployeeBasicDTO> DeleteUserByUId(string UId)
        {
            var existingUser = await _cosmosDBService.GetEmployeeBasicDetailsByUId(UId);
            existingUser.Archieved = true;
            await _cosmosDBService.ReplaceAsync(UId, existingUser);

            existingUser.Initialize(false, "Employee", "Sudh", "Sudh");
            existingUser.Active = false;
            existingUser.Archieved = true;
            var response = await _cosmosDBService.AddEmployeeBasicDetails(existingUser);
            return _mapper.Map<EmployeeBasicDTO>(response);

        }

        public async Task<EmployeeBasicDTO> AddEmployeeBasicDetails(EmployeeBasicDTO employee)
        {
            var entity = _mapper.Map<EmployeeBasicEntity>(employee);
            entity.Initialize(true, "Employee", "Sudh", "Sudh");

            var response = await _cosmosDBService.AddEmployeeBasicDetails(entity); // Await the async operation

            return _mapper.Map<EmployeeBasicDTO>(response);
        }

        public async Task<List<EmployeeBasicEntity>> GetAllEmployeeBasicDetailsByRole(string role)
        {
            try
            {
                var allEmployees = await GetAllEmployeeBasicDetails();
                return allEmployees.FindAll(a => a.Role == role);
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error in GetAllEmployeeBasicDetailsByRole", ex);
            }
        }

        public async Task<EmployeeFilterCriteria> GetAllEmployeeBasicDetailsByPagination(EmployeeFilterCriteria employeeFilterCriteria)
        {
            EmployeeFilterCriteria criteria = new EmployeeFilterCriteria();

            var employee = await GetAllEmployeeBasicDetails();
            criteria.totalCount = employee.Count;
             
            var skip = employeeFilterCriteria.PageSize * (employeeFilterCriteria.Page - 1);

            employee = employee.Skip(skip).Take(employeeFilterCriteria.PageSize).ToList();

            employeeFilterCriteria.Employee = employee;

            return criteria;
        }

        public async Task<EmployeeBasicDTO> AddstudentByMakePostRequest(EmployeeBasicDTO employeeBasicDTO)
        {

            var serialixedObj = JsonConvert.SerializeObject(employeeBasicDTO);
            var requestObj = await HttpClientHelper.MakePostRequest(Credentials.EmployeeUrl, Credentials.AddEmployeeEndpoint, serialixedObj);
            var responseObj = JsonConvert.DeserializeObject<EmployeeBasicDTO>(requestObj);
            return responseObj;
        }
        public async Task<List<EmployeeBasicDTO>> GetAllStudentByMakeGetRequest()
        {
            var request = await HttpClientHelper.MakeGetRequest(Credentials.EmployeeUrl, Credentials.GetEmployeeEndpoint);
            return JsonConvert.DeserializeObject<List< EmployeeBasicDTO>>(request);
        }
    }
}