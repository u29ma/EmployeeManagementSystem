namespace EmployeeManagementSystem.ReportsViewModels
{
    public class AttendanceReportVM
    {
        public int AttendanceId { get; set; }

        public string EmployeeName { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? CheckIn { get; set; }

        public TimeSpan? CheckOut { get; set; }

        public string Status { get; set; }
    }
}
