using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HOA.Models;
using HOA.Services.Interfaces;

namespace HOA.Controllers
{
    public class AnnouncementsController : Controller
    {
        private IAnnouncementsService _announcementsService;


        public AnnouncementsController(IAnnouncementsService announcementsService)
        {
            _announcementsService = announcementsService;
        }

        // GET: Announcements
        public IActionResult Index(string searchQuery)
        {
            ViewData["SearchQuery"] = searchQuery;
            var announcements = string.IsNullOrEmpty(searchQuery)
                ? _announcementsService.GetAllAnnouncements()
                : _announcementsService.SearchAnnouncementsByTitle(searchQuery);
            return View(announcements);
        }

        // GET: Announcements/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcements = _announcementsService.GetAnnouncementsById((int)id);

            if (announcements == null)
            {
                return NotFound();
            }

            return View(announcements);
        }

        // GET: Announcements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Description,Date")] Announcement announcements)
        {
            if (ModelState.IsValid)
            {
                _announcementsService.AddAnnouncement(announcements);
                return RedirectToAction(nameof(Index));
            }
            return View(announcements);
        }

        // GET: Announcements/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcements = _announcementsService.GetAnnouncementsById((int)id);
            
            if (announcements == null)
            {
                return NotFound();
            }
            return View(announcements);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Description,Date")] Announcement announcements)
        {
            if (id != announcements.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _announcementsService.UpdateAnnouncement(announcements);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementExists(announcements.Id))
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
            return View(announcements);
        }

        // GET: Announcements/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcements = _announcementsService.GetAnnouncementsById((int)id);

            if (announcements == null)
            {
                return NotFound();
            }

            return View(announcements);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var announcements = _announcementsService.GetAnnouncementsById(id);
            if (announcements != null)
            {
                _announcementsService.DeleteAnnouncement(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AnnouncementExists(int id)
        {
            return _announcementsService.GetAnnouncementsById(id) != null;
        }
    }
}
