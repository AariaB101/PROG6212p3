using Microsoft.AspNetCore.Mvc;
using PROG6212p3.Models;
using System.Linq;

namespace PROG6212p3.Controllers
{
    public class CoordinatorPageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoordinatorPageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var claims = _context.Claims.ToList(); // Load the claims from DB
            return View(claims); // ✅ Send this to the view
        }

        [HttpPost]
        public IActionResult Approve(int claimId)
        {
            var claim = _context.Claims.Find(claimId);
            if (claim != null && claim.Status == ClaimStatus.Pending)
            {
                claim.Status = ClaimStatus.ApprovedByPC;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Reject(int claimId)
        {
            var claim = _context.Claims.Find(claimId);
            if (claim != null && claim.Status == ClaimStatus.Pending)
            {
                claim.Status = ClaimStatus.Rejected;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
