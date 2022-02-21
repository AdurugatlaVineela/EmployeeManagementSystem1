using System;
using System.ComponentModel.DataAnnotations;
namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        [Key]
        // [Required(ErrorMessage ="EmployeeId is required")]
        // [Range(minimum:1,maximum:20,ErrorMessage ="limit exceeded")]
        public int EmployeeId{get; set;}
        [Required]
        [StringLength(maximumLength:20, ErrorMessage ="Maximum length exceeded")]
        public string LastName{get; set;}
        [Required]
        [StringLength(maximumLength:10, ErrorMessage ="Maximum length exceeded")]
        public string FirstName{get; set;}
        public DateTime? BirthDate{get; set;}
        public DateTime? HireDate{get; set;}
        public string? Address{get; set;}
        public string? City{get; set;}
        public string? Region{get; set;}
        public int? PostalCode{get; set;}
        public string? Country{get; set;}
        public int? HomePhone{get; set;}
        public int? DepartmentId{get; set;}
        public int? ProjectId{get; set;}
    }
}