using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HOA.Models;
using Microsoft.AspNetCore.Authorization;

namespace HOA.Controllers
{
    [Authorize]
    public class ResidentsController : Controller
    {
        
        private IResidentsService _residentsService;

        public ResidentsController(IResidentsService residentsService)
        {
            _residentsService = residentsService;
        }

        
        // GET: Residents
        public IActionResult Index(string searchQuery)
        {
            ViewData["SearchQuery"] = searchQuery;

            var residents = string.IsNullOrEmpty(searchQuery)
                ? _residentsService.GetAllResidents()
                : _residentsService.SearchResidentsByName(searchQuery);

            return View(residents);
        }

        // GET: Residents/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resident = _residentsService.GetResidentById((int)id);
            if (resident == null)
            {
                return NotFound();
            }

            return View(resident);
        }

        [Authorize(Roles = "Admin")]
        // GET: Residents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Residents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Apartment,PhoneNumber,Email,Status")] Resident resident)
        {
            if (ModelState.IsValid)
            {
                _residentsService.AddResident(resident);
                return RedirectToAction(nameof(Index));
            }
            return View(resident);
        }

        [Authorize(Roles = "Admin")]
        // GET: Residents/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resident = _residentsService.GetResidentById((int)id);
            if (resident == null)
            {
                return NotFound();
            }
            return View(resident);
        }

        [Authorize(Roles = "Admin")]
        // POST: Residents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Apartment,PhoneNumber,Email,Status")] Resident resident)
        {
            if (id != resident.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _residentsService.UpdateResident(resident);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResidentExists(resident.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(resident);
        }

        [Authorize(Roles = "Admin")]
        // GET: Residents/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resident = _residentsService.GetResidentById((int)id);
            if (resident == null)
            {
                return NotFound();
            }

            return View(resident);
        }

        [Authorize(Roles = "Admin")]
        // POST: Residents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var resident = _residentsService.GetResidentById(id);
            if (resident != null)
            {
               _residentsService.DeleteResident(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ResidentExists(int id)
        {
            return _residentsService.GetResidentById(id) != null;
        }

    }
}
