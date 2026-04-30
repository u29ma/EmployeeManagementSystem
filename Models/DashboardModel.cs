namespace EmployeeManagementSystem.Models
{
    public class DashboardModel
    {
        public int TotalEmployees { get; set; }
        public int OnLeaveToday { get; set; }
        public int TotalDepartments { get; set; }
        public int PendingApprovals { get; set; }

        public int PresentToday { get; set; }
        public int TotalAnnouncements { get; set; }
        public int ApprovedLeaves { get; set; }
        public int PendingPayroll { get; set; }
    }
    public class EmployeeDashboardModel
    {
        public int TotalLeaves { get; set; }
        public int ApprovedLeaves { get; set; }
        public int PendingLeaves { get; set; }
        public int PresentDays { get; set; }
    }

}
