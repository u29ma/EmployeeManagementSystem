using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.ReportsViewModels;

namespace EmployeeManagementSystem.Da
{
    public class ReportsDa
    {
        private readonly ApplicationDbContext _context;
        public ReportsDa(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<EmployeeReportVM> GetEmployeeReport(string search)
        {
            var query = from e in _context.Employees
                        join d in _context.Departments
                        on e.DepartmentId equals d.DepartmentId
                        select new EmployeeReportVM
                        {
                            EmployeeId = e.EmployeeId,
                            FullName = e.FirstName + " " + e.LastName,
                            DepartmentName = d.DepartmentName,
                            Designation = e.Designation,
                            JoinDate = e.JoiningDate
                        };

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.FullName.Contains(search));
            }

            return query.ToList();
        }
    }
}
