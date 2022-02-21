//Folder: Models

using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class User
    {
        [Key]
        public int Id {get; set;}
        [Required]
        [StringLength(maximumLength:20, ErrorMessage ="Maximum length exceeded")]

        public string UserName{get; set;}
        [Required]
        [StringLength(maximumLength:20, ErrorMessage ="length limit exceeded")]

        public string Password {get; set;}

        public void Deconstruct(out string userName, out string password)
        {
            userName=this.UserName;
            password=this.Password;
        }
    }
    public class Role
    {
        [Key]
        public int RoleId {get; set;}
        public string RoleName {get; set;}
    }

    public enum RoleNames
    {
        admin,
        user,
        employee
    }
}