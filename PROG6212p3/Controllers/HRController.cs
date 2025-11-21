using Microsoft.AspNetCore.Mvc;
using PROG6212p3.Models;
using System.Linq;

namespace PROG6212p3.Controllers
{
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -----------------------
        // Dashboard - list all lecturers
        // -----------------------
        public IActionResult Index()
        {
            var lecturers = _context.Lecturers.ToList();
            return View(lecturers); // returns a list to the Index view
        }

        // -----------------------
        // GET: Add a new lecturer
        // -----------------------
        [HttpGet]
        public IActionResult AddLecturer()
        {
            return View(new Lecturer()); // returns a single lecturer model
        }

        // -----------------------
        // POST: Save the new lecturer
        // -----------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLecturer(Lecturer lecturer)
        {
            if (!ModelState.IsValid)
            {
                return View(lecturer); // validation failed, show form again
            }

            _context.Lecturers.Add(lecturer);
            _context.SaveChanges();

            TempData["Success"] = $"Lecturer '{lecturer.FullName}' added successfully!";
            return RedirectToAction("Index");
        }

        // -----------------------
        // Optional: Approve claim
        // -----------------------
        [HttpPost]
        public IActionResult Approve(int claimId)
        {
            var claim = _context.Claims.Find(claimId);
            if (claim != null)
            {
                claim.Status = ClaimStatus.ApprovedByAM;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // -----------------------
        // Optional: Reject claim
        // -----------------------
        [HttpPost]
        public IActionResult Reject(int claimId)
        {
            var claim = _context.Claims.Find(claimId);
            if (claim != null)
            {
                claim.Status = ClaimStatus.Rejected;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
