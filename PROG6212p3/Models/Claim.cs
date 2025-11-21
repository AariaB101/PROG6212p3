using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PROG6212p3.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }

        [Required]
        [Range(1, 100)]
        public int HoursWorked { get; set; }

        [Required]
        [Range(1, 10000)]
        public decimal HourlyRate { get; set; }

        public decimal Total => HoursWorked * HourlyRate;

        public string? LecturerName { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public ClaimStatus? Status { get; set; }

        public string? Notes { get; set; }
        public string? DocumentPath { get; set; }
    }
    public enum ClaimStatus
    {
        Pending,
        ApprovedByPC,
        ApprovedByAM,
        Rejected
    }
}

