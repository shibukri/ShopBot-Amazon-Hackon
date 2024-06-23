using AutoMapper;
using Employee_Management_System.Common;
using Employee_Management_System.CosmosDB;
using Employee_Management_System.DTO;
using Employee_Management_System.Entities;
using Employee_Management_System.Interfaces;

public class EmployeeAdditionalService : IEmployeeAdditionalService
{
    private readonly ICosmosDBService _cosmosDBService;
    private readonly IMapper _mapper;

    public EmployeeAdditionalService(ICosmosDBService cosmosDBService, IMapper mapper)
    {
        _cosmosDBService = cosmosDBService;
        _mapper = mapper;
    }

    public async Task<EmployeeAdditionalEntity> AddEmployeeAdditionalDetails(EmployeeAdditionalEntity additionalDetailsEntity)
    {
        additionalDetailsEntity.Initialize(true, "Additional", "admin", "admin");
        additionalDetailsEntity = await _cosmosDBService.AddEmployeeAdditionalDetails(additionalDetailsEntity);
        return additionalDetailsEntity;
    }

    public async Task<List<EmployeeAdditionalEntity>> GetAllEmployeeAdditionalDetails()
    {
        return await _cosmosDBService.GetAllEmployeeAdditionalDetails();
    }

    public async Task<EmployeeAdditionalDTO> GetEmployeeAdditionalDetailsByBasicDetailsUId(string basicDetailsUId)
    {
        var response = await _cosmosDBService.GetEmployeeAdditionalDetailsByBasicDetailsUId(basicDetailsUId);
        return _mapper.Map<EmployeeAdditionalDTO>(response);
    }

    public async Task<EmployeeAdditionalDTO> UpdateAdditionalDetailsByBasicDetailsUId(string UId, EmployeeAdditionalDTO additionalDetailsDTO)
    {
        var existingAdditionalDetails = await _cosmosDBService.GetEmployeeAdditionalDetailsByBasicDetailsUId(UId);
        if (existingAdditionalDetails == null)
        {
            return null;
        }
        existingAdditionalDetails.Active = false;
        existingAdditionalDetails.Archieved = true;

       
        await _cosmosDBService.ReplaceAdditionalDetailsAsync(UId, existingAdditionalDetails);
        existingAdditionalDetails.Initialize(true, Credentials.AdditionalEmployee, "Sudh", "Sudh");

        _mapper.Map(additionalDetailsDTO, existingAdditionalDetails);
        existingAdditionalDetails =await _cosmosDBService.AddEmployeeAdditionalDetails(existingAdditionalDetails);
        return _mapper.Map<EmployeeAdditionalDTO>(existingAdditionalDetails);
    }


    public async Task<EmployeeAdditionalDTO> DeleteAdditionalDetailsByBasicDetailsUId(string basicDetailsUId)
    {
        var existingAdditionalDetails = await _cosmosDBService.GetEmployeeAdditionalDetailsByBasicDetailsUId(basicDetailsUId);
        if (existingAdditionalDetails == null)
        {
            return null;
        }
        existingAdditionalDetails.Active = false;
        existingAdditionalDetails.Archieved = true;
        await _cosmosDBService.ReplaceAdditionalDetailsAsync(basicDetailsUId, existingAdditionalDetails);

        return _mapper.Map<EmployeeAdditionalDTO>(existingAdditionalDetails);
    }
}
