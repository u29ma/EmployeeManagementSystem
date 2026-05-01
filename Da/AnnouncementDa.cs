using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagementSystem.Da
{
    public class AnnouncementDa
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementDa(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get all (Admin)
        public List<AnnouncementModel> GetAll()
        {
            return _context.Announcements
                .OrderByDescending(a => a.CreatedAt)
                .ToList();
        }

        // ✅ Add
        public void Add(AnnouncementModel model)
        {
            _context.Announcements.Add(model);
            _context.SaveChanges();
        }

        // ✅ Get for Employee
        public List<AnnouncementModel> GetForEmployee(string role)
        {
            return _context.Announcements
                .Where(a =>
                    (a.TargetRole == "All" || a.TargetRole == role) &&
                    (a.ExpiryDate == null || a.ExpiryDate >= DateTime.Now) &&
                    a.IsActive == true
                )
                .OrderByDescending(a => a.CreatedAt)
                .ToList();
        }

        public List<AnnouncementModel> GetAllAnnouncements()
        {
            return _context.Announcements
                .OrderByDescending(a => a.AnnouncementId)
                .ToList();
        }

    }

}
