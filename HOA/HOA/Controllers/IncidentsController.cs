using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HOA.Models;
using HOA.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HOA.Controllers
{
    [Authorize]
    public class IncidentsController : Controller
    {
        private IIncidentsService _incidentsService;
        private readonly INotificationService _notificationService;

        public IncidentsController(IIncidentsService incidentsService, INotificationService notificationService)
        {
            _incidentsService = incidentsService;
            _notificationService = notificationService;
        }

        // GET: Incidents
        public IActionResult Index(string searchQuery, string sortOrder)
        {
            ViewData["SearchQuery"] = searchQuery;
            ViewData["SortOrder"] = sortOrder;

            IEnumerable<Incident> incidents;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                incidents = _incidentsService.SearchIncidentsByIncidentName(searchQuery);
            }
            else
            {
                bool descending = sortOrder == "date_desc";
                incidents = _incidentsService.SortIncidentsByDate(descending);
            }

            return View(incidents);
        }


        // GET: Incidents/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var incident = _incidentsService.GetIncidentsById((int)id);

            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        [Authorize(Roles = "Admin")]
        // GET: Incidents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Description,Date,Status,Location,ImagePath")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                _incidentsService.AddIncident(incident);
                TempData["ConfirmationMessage"] = "Incident reported successfully. Thank you for your submission.";

                _notificationService.CreateNotification(new Notification
                {
                    Message = $"New incident: {incident.Title.Substring(0, Math.Min(6, incident.Title.Length))}...",
                    Link = Url.Action("Details", "Incidents", new { id = incident.Id }),
                    Timestamp = DateTime.Now
                });
                return RedirectToAction(nameof(Index));
            }
            return View(incident);
        }

        [Authorize(Roles = "Admin")]
        // GET: Incidents/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = _incidentsService.GetIncidentsById((int)id);

            if (incident == null)
            {
                return NotFound();
            }
            return View(incident);
        }

        [Authorize(Roles = "Admin")]
        // POST: Incidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Description,Date,Status,Location,ImagePath")] Incident incident)
        {
            if (id != incident.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _incidentsService.UpdateIncident(incident);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentExists(incident.Id))
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
            return View(incident);
        }

        [Authorize(Roles = "Admin")]
        // GET: Incidents/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = _incidentsService.GetIncidentsById((int)id);

            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        [Authorize(Roles = "Admin")]
        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var incident = _incidentsService.GetIncidentsById(id);

            if (incident != null)
            {
                _incidentsService.DeleteIncident(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool IncidentExists(int id)
        {
            return _incidentsService.GetIncidentsById(id) != null;
        }
    }
}
