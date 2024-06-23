using Employee_Management_System.Common;
using Employee_Management_System.DTO;
using Employee_Management_System.Entities;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using Container = Microsoft.Azure.Cosmos.Container;


namespace Employee_Management_System.CosmosDB
{
    public class CosmosDBService : ICosmosDBService
    {

        public Container _container;

        public CosmosDBService()
        {
            _container = GetContainer();
        }
        private Container GetContainer()
        {
            CosmosClient cosmosclient = new CosmosClient(Credentials.CosmosEndpoint, Credentials.PrimaryKey);
            Database database = cosmosclient.GetDatabase(Credentials.DatabaseName);
            Container container = database.GetContainer(Credentials.ContainerName);
            return container;
        }


        public async Task<EmployeeBasicEntity> AddEmployeeBasicDetails(EmployeeBasicEntity employeeEntity)
        {
           return await _container.CreateItemAsync(employeeEntity);
        }

       

        public async Task<List<EmployeeBasicEntity>> GetAllEmployeeBasicDetails()
        {
            var response = _container.GetItemLinqQueryable<EmployeeBasicEntity>(true).Where(q => q.DocumentType == Credentials.EmployeeDataType && q.Active ==true && q.Archieved == false).AsEnumerable().ToList();
            return response;
        }

       

        public async Task<EmployeeBasicEntity> GetEmployeeBasicDetailsByUId(string UId)
        {
            EmployeeBasicEntity response = _container.GetItemLinqQueryable<EmployeeBasicEntity>(true).Where(q => q.UId == UId && q.DocumentType == Credentials.EmployeeDataType && q.Active && !q.Archieved).AsEnumerable().FirstOrDefault();
            return response;
        }

        public async Task ReplaceAsync(string UId, EmployeeBasicEntity existingUser)
        {
            await _container.ReplaceItemAsync(existingUser, UId);
        }

        public async Task<EmployeeAdditionalEntity> AddEmployeeAdditionalDetails(EmployeeAdditionalEntity additionalDetailsEntity)
        {
            return await _container.CreateItemAsync(additionalDetailsEntity);
        }

        public async Task<List<EmployeeAdditionalEntity>> GetAllEmployeeAdditionalDetails()
        {
            var response = _container.GetItemLinqQueryable<EmployeeAdditionalEntity>(true).Where(q => q.DocumentType == "AdditionalDetails" && q.Active && !q.Archieved).AsEnumerable().ToList();
            return response;
        }

        public async Task<EmployeeAdditionalEntity> GetEmployeeAdditionalDetailsByBasicDetailsUId(string basicDetailsUId)
        {
            var response = _container.GetItemLinqQueryable<EmployeeAdditionalEntity>(true).Where(q => q.EmployeeBasicDetailsUId == basicDetailsUId && q.Active && !q.Archieved).AsEnumerable().FirstOrDefault();

            return response;
        }

        public async Task ReplaceAdditionalDetailsAsync(string basicDetailsUId, EmployeeAdditionalEntity additionalDetailsEntity)
        {
            await _container.ReplaceItemAsync(additionalDetailsEntity, basicDetailsUId);
        }

        public async Task<EmployeeBasicDTO> AddEmployeeBasicDetails(EmployeeBasicDTO employeeBasicDetailsDTO)
        {
            var response = await _container.CreateItemAsync(employeeBasicDetailsDTO);
            return response;
        }

       
    }
}
