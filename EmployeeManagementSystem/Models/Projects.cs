using System;
using System.ComponentModel.DataAnnotations;
namespace EmployeeManagementSystem.Models
{

    public class Project
    {
        [Key]
        [Required(ErrorMessage ="projectId is required")]
        [Range(minimum:1,maximum:10,ErrorMessage ="limit exceeded")]
        public int ProjectId{get; set;}
        [Required]
        [StringLength(maximumLength:20, ErrorMessage ="Maximum length exceeded")]
        public string ProjectName{get; set;}
        public string? ProjectLocation{get; set;}
        public string ProjectDescription{get; set;}
        public DateTime? StartDate{get; set;}
        public DateTime? EndDate{get; set;}
    }
}