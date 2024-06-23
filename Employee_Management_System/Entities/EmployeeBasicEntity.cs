using Employee_Management_System.Common;
using Newtonsoft.Json;
namespace Employee_Management_System.Entities
{
    public class EmployeeBasicEntity : BaseEntity
    {

        [JsonProperty(PropertyName = "salutory", NullValueHandling = NullValueHandling.Ignore)]
        public string Salutory { get; set; }

        [JsonProperty(PropertyName = "firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "middleName", NullValueHandling = NullValueHandling.Ignore)]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "nickName", NullValueHandling = NullValueHandling.Ignore)]
        public string NickName { get; set; }

        [JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "mobile", NullValueHandling = NullValueHandling.Ignore)]

        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "employeeID", NullValueHandling = NullValueHandling.Ignore)]

        public string EmployeeID { get; set; }

        [JsonProperty(PropertyName = "role", NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "reportingManagerUId", NullValueHandling = NullValueHandling.Ignore)]
        public string ReportingManagerUId { get; set; }

        [JsonProperty(PropertyName = "reportingManagerName", NullValueHandling = NullValueHandling.Ignore)]
        public string ReportingManagerName { get; set; }

        [JsonProperty(PropertyName = "address", NullValueHandling = NullValueHandling.Ignore)]

        public Address Address { get; set; }

    }
    public class Address
    {
        public int HouseNumber { get; set; }
        public string StreetName { get; set; }
        public string Landmark { get; set; }
        public string PostalCodes { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }


    public class EmployeeFilterCriteria
    {
        public EmployeeFilterCriteria() {
            Filters = new List<FilterCriteria>();
            Employee = new List<EmployeeBasicEntity>();
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int totalCount { get; set; }
        public List<FilterCriteria> Filters { get; set; }
        public List<EmployeeBasicEntity> Employee { get; set; }
    }

    public class FilterCriteria
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
}