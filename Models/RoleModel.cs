using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class RoleModel
    {
        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}
