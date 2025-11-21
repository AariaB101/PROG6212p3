using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212p3.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Claim = PROG6212p3.Models.Claim;


public class LecturerPageController : Controller
{
    private readonly ApplicationDbContext _context;

    public LecturerPageController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Show claim submission form
    [HttpGet]
    public IActionResult Claims()
    {
        return View("~/Views/LecturerPage/Claims.cshtml");
    }

    // POST: Handle claim submission
    [HttpPost]
    public async Task<IActionResult> Claims(Claim claim, IFormFile? document)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/LecturerPage/Claims.cshtml", claim);
        }

        try
        {
            // Pull lecturer info from DB
            var lecturer = _context.Lecturers.FirstOrDefault(l => l.UserName == User.Identity.Name);
            if (lecturer == null)
            {
                ModelState.AddModelError("", "Lecturer not found.");
                return View("~/Views/LecturerPage/Claims.cshtml", claim);
            }

            claim.LecturerName = lecturer.FullName;
            claim.HourlyRate = lecturer.HourlyRate;

            // Validation: max 180 hours
            if (claim.HoursWorked > 180)
            {
                ModelState.AddModelError("HoursWorked", "You cannot submit more than 180 hours per month.");
                return View("~/Views/LecturerPage/Claims.cshtml", claim);
            }

            claim.SubmittedOn = DateTime.Now;
            claim.Status = ClaimStatus.Pending;

            // Handle file upload
            if (document != null && document.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                var extension = Path.GetExtension(document.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Document", "Only .pdf, .docx, or .xlsx files are allowed.");
                    return View("~/Views/LecturerPage/Claims.cshtml", claim);
                }

                if (document.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("Document", "File size must be under 5MB.");
                    return View("~/Views/LecturerPage/Claims.cshtml", claim);
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + extension;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await document.CopyToAsync(stream);
                }

                claim.DocumentPath = "/uploads/" + fileName;
            }

            // Add to DB
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Claim submitted successfully!";
            return RedirectToAction("Lecturer");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error saving claim: " + ex.Message);
            return View("~/Views/LecturerPage/Claims.cshtml", claim);
        }
    }

    // GET: Lecturer dashboard showing all claims
    public IActionResult Lecturer()
    {
        // Filter by currently logged-in lecturer
        var lecturerName = User.Identity.Name; // Make sure authentication is set up
        var claims = _context.Claims
                             .Where(c => c.LecturerName == lecturerName)
                             .ToList();
        return View("~/Views/LecturerPage/Lecturer.cshtml", claims);
    }

}
