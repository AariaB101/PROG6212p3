using PROG6212p3.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PROG6212p3.Models
{
    public class Lecturer
    {
        [Key]
        public int LecturerId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; }

        public string Email { get; set; }

        public decimal HourlyRate { get; set; }

        public enum Role { get, set, }




        // Navigation property
        public ICollection<Claim> Claim { get; set; }
    }

}

