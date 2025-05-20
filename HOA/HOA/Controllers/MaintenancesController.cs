using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HOA.Models;
using HOA.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HOA.Controllers
{
    [Authorize]
    public class MaintenancesController : Controller
    {
        private IMaintenanceService _maintenanceService;

        public MaintenancesController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        // GET: Maintenances
        public IActionResult Index(string searchQuery)
        {
            ViewData["SearchQuery"] = searchQuery;
            var maintenances = string.IsNullOrEmpty(searchQuery)
                ? _maintenanceService.GetAllMaintenance()
                : _maintenanceService.SearchMaintenanceByTaskName(searchQuery);
            return View(maintenances);
        }

        // GET: Maintenances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = _maintenanceService.GetMaintenanceById((int)id);

            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        [Authorize(Roles = "Admin")]
        // GET: Maintenances/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Maintenances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,TaskName,AssignedPersonnel,DueDate,Status")] Maintenance maintenance)
        {
            if (ModelState.IsValid)
            {
                _maintenanceService.AddMaintenance(maintenance);
                return RedirectToAction(nameof(Index));
            }
            return View(maintenance);
        }

        [Authorize(Roles = "Admin")]
        // GET: Maintenances/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = _maintenanceService.GetMaintenanceById((int)id);
            
            if (maintenance == null)
            {
                return NotFound();
            }
            return View(maintenance);
        }

        [Authorize(Roles = "Admin")]
        // POST: Maintenances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,TaskName,AssignedPersonnel,DueDate,Status")] Maintenance maintenance)
        {
            if (id != maintenance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _maintenanceService.UpdateMaintenance(maintenance);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaintenanceExists(maintenance.Id))
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
            return View(maintenance);
        }

        [Authorize(Roles = "Admin")]
        // GET: Maintenances/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = _maintenanceService.GetMaintenanceById((int)id);
            
            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        [Authorize(Roles = "Admin")]
        // POST: Maintenances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var maintenance = _maintenanceService.GetMaintenanceById(id);
            
            if (maintenance != null)
            {
                _maintenanceService.DeleteMaintenance(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MaintenanceExists(int id)
        {
            return _maintenanceService.GetMaintenanceById(id) != null;
        }
    }
}
