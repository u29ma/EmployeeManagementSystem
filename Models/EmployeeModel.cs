using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models;
public class EmployeeModel
{
    [Key] public int EmployeeId { get; set; }
    public int UserId { get; set; } // ✅ Foreign key
    public string? Email { get; set; } 
    public string? Password { get; set; } 
    
    //[ForeignKey("UserId")]
    //public UserModel User { get; set; } 
    public string FirstName { get; set; } 
    public string LastName { get; set; } 
    public int DepartmentId { get; set; } // ✅ Foreign key

    //[ForeignKey("DepartmentId")] 
    //public DepartmentModel Department { get; set; }
    public string Gender { get; set; } 
    public DateTime DOB { get; set; } 
    public string Phone { get; set; }
    public string Address { get; set; } 
    public string Designation { get; set; } 
    public DateTime JoiningDate { get; set; }
    public bool IsProfileComplete { get; set; } = false;

    [Column(TypeName = "decimal(18,2)")] 
    public decimal Salary { get; set; } 
    public bool Status { get; set; } // 1 / 0
}

