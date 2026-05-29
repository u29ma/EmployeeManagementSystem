
namespace EmployeeManagementSystem.Models
{
    public class ProfileViewModel
    {
        public EmployeeModel Employee { get; set; }
        public string Email { get; set; }
        public int PresentDays { get; set; }
        public int TotalLeaves { get; set; }
        public int PayrollGenerated { get; set; }
    }
}
