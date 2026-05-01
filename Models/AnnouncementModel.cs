using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models;
public class AnnouncementModel
{
    [Key]
    public int AnnouncementId { get; set; }

    public string? Title { get; set; }
    public string? Message { get; set; }

    public int CreatedBy { get; set; }

    // ✅ New Fields
    public DateTime CreatedAt { get; set; }   // when created
    public string? Priority { get; set; }     // High / Medium / Low
    public bool IsActive { get; set; }        // true / false

    // (Optional but recommended if already added in DB)
    public DateTime? ExpiryDate { get; set; }
    public string? TargetRole { get; set; }
}
