namespace Employee_Management_System.Common
{
    public class Credentials
    {
        public static readonly string DatabaseName = Environment.GetEnvironmentVariable("databaseName");
        public static readonly string ContainerName = Environment.GetEnvironmentVariable("containerName");
        public static readonly string CosmosEndpoint = Environment.GetEnvironmentVariable("cosmosUrl");
        public static readonly string PrimaryKey = Environment.GetEnvironmentVariable("primaryKey");
        internal static readonly string EmployeeUrl = Environment.GetEnvironmentVariable("employeeUrl");
        internal static readonly string AddEmployeeEndpoint = "/api/Employee/AddEmployeeBasicDetails";
        internal static readonly string GetEmployeeEndpoint = "/api/Employee/GetAllEmployeeBasicDetails";
        public static readonly string EmployeeDataType = "Employee";
        public static readonly string AdditionalEmployee = "AdditionalEmployee";
    }
}


