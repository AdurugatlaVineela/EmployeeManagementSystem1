using System.ComponentModel.DataAnnotations;
namespace EmployeeManagementSystem.Models
{
    public class Company
    {
        [Key]
        [Required(ErrorMessage ="CompanyId is required")]
        [Range(minimum:1,maximum:10000,ErrorMessage ="limit exceeded")]
        public int CompanyId{get; set;}

        public string? CompanyName{get; set;}
        public string? CompanyAddress{get; set;}
        public string? City{get; set;}
        public string? PostalCode{get; set;}
        public string? Country{get; set;}
    }
}