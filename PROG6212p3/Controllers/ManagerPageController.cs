using Microsoft.AspNetCore.Mvc;
using PROG6212p3.Models;
using System.Linq;

namespace PROG6212p3.Controllers
{
    public class ManagerPageController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ManagerPageController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var claims = _db.Claims.ToList();
            return View(claims);
        }

        [HttpPost]
        public IActionResult Approve(int claimId)
        {
            var claim = _db.Claims.Find(claimId);
            if (claim != null && claim.Status == ClaimStatus.ApprovedByPC)
            {
                claim.Status = ClaimStatus.ApprovedByAM;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Reject(int claimId)
        {
            var claim = _db.Claims.Find(claimId);
            if (claim != null)
            {
                claim.Status = ClaimStatus.Rejected;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
