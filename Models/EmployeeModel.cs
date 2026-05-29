using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models;
public class EmployeeModel
{
    [Key] public int EmployeeId { get; set; }
    public int? RoleId { get; set; }
    [NotMapped]
    public string? RoleName { get; set; }
    public string FirstName { get; set; } 
    public string LastName { get; set; } 
    public int? DepartmentId { get; set; }
    [NotMapped]
    public string? DepartmentName { get; set; }
    public string? Gender { get; set; } 
    public DateTime DOB { get; set; } 
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public int? DesignationId { get; set; }
    [NotMapped]
    public string? DesignationName { get; set; }
    public DateTime JoiningDate { get; set; }
   
    [Column(TypeName = "decimal(18,2)")] 
    public decimal Salary { get; set; } 
    public bool Status { get; set; } // 1 / 0
}

