using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models;
public class DepartmentModel
{
    [Key]
    public int DepartmentId { get; set; }

    [Required]
    public string DepartmentName { get; set; }

    public string? Description { get; set; }

    //public List<EmployeeModel>? Employees { get; set; }
}
