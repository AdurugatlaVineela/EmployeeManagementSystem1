namespace EmployeeManagementSystem.Models
{
    public class ServiceBusMessageData
    {
        public string FirstName { get; set; }

        public string CompanyName { get; set; }

        public string ProjectName { get; set; }

        public string action { get; set; }

        public string actionMessage {get;set;}
        public string DepartmentName { get; internal set; }
        public string UserName { get; internal set; }
    }
}
