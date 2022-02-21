using System.ComponentModel.DataAnnotations;
namespace EmployeeManagementSystem.Models
{
    public class Department
    {
        [Key]
        [Required(ErrorMessage ="DepartmentId is required")]
        [Range(minimum:1,maximum:10,ErrorMessage ="limit exceeded")]
        public int DepartmentId{get; set;}
        public string DepartmentName{get; set;}
        public string? DepartmentLocation{get; set;}
    }
}