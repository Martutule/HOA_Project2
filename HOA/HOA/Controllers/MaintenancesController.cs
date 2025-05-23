using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HOA.Models;
using HOA.Services.Interfaces;
using HOA.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace HOA.Controllers
{
    [Authorize]
    public class MaintenancesController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly ISupplierContractService _supplierContractService;

        public MaintenancesController(IMaintenanceService maintenanceService, ISupplierContractService supplierContractService)
        {
            _maintenanceService = maintenanceService;
            _supplierContractService = supplierContractService;
        }

        // GET: Maintenances
        public IActionResult Index(string searchQuery)
        {
            ViewData["SearchQuery"] = searchQuery;
            var maintenances = string.IsNullOrEmpty(searchQuery)
                ? _maintenanceService.GetAllMaintenance()
                : _maintenanceService.SearchMaintenanceByTaskName(searchQuery);

            var supplierContracts = _supplierContractService.GetAllContracts();

            var viewModel = new MaintenanceIndexViewModel
            {
                Maintenances = maintenances,
                SupplierContracts = supplierContracts
            };

            return View(viewModel);
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

        // --- Supplier Contract CRUD actions (optional, for completeness) ---

        [Authorize(Roles = "Admin")]
        // GET: Maintenances/CreateSupplierContract
        public IActionResult CreateSupplierContract()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Maintenances/CreateSupplierContract
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSupplierContract([Bind("Id,SupplierName,Category,ContractStartDate,ContractEndDate")] SupplierContract contract)
        {
            if (ModelState.IsValid)
            {
                _supplierContractService.AddContract(contract);
                return RedirectToAction(nameof(Index));
            }
            return View(contract);
        }

        [Authorize(Roles = "Admin")]
        // GET: Maintenances/EditSupplierContract/5
        public IActionResult EditSupplierContract(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = _supplierContractService.GetContractById((int)id);

            if (contract == null)
            {
                return NotFound();
            }
            return View(contract);
        }

        [Authorize(Roles = "Admin")]
        // POST: Maintenances/EditSupplierContract/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSupplierContract(int id, [Bind("Id,SupplierName,Category,ContractStartDate,ContractEndDate")] SupplierContract contract)
        {
            if (id != contract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _supplierContractService.UpdateContract(contract);
                return RedirectToAction(nameof(Index));
            }
            return View(contract);
        }

        [Authorize(Roles = "Admin")]
        // GET: Maintenances/DeleteSupplierContract/5
        public IActionResult DeleteSupplierContract(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = _supplierContractService.GetContractById((int)id);

            if (contract == null)
            {
                return NotFound();
            }
            return View(contract);
        }

        [Authorize(Roles = "Admin")]
        // POST: Maintenances/DeleteSupplierContract/5
        [HttpPost, ActionName("DeleteSupplierContract")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSupplierContractConfirmed(int id)
        {
            var contract = _supplierContractService.GetContractById(id);

            if (contract != null)
            {
                _supplierContractService.DeleteContract(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
