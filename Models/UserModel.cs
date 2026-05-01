using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models;
public class UserModel
{
    [Key]
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public int EmployeeId { get; set; }
}
