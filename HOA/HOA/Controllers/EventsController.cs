using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HOA.Models;
using HOA.Services.Interfaces;

namespace HOA.Controllers
{
    public class EventsController : Controller
    {
        private IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        // GET: Events
        public IActionResult Index(string searchQuery)
        {
            ViewData["SearchQuery"] = searchQuery;
            var events = string.IsNullOrEmpty(searchQuery)
                ? _eventsService.GetAllEvents()
                : _eventsService.SearchEventsByEventName(searchQuery);
            return View(events);
        }

        // GET: Events/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _eventsService.GetEventById((int) id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ImagePath,EventName,Description,Location,Date,Color")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _eventsService.AddEvent(@event);

                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _eventsService.GetEventById((int) id);

            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,ImagePath,EventName,Description,Location,Date,Color")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _eventsService.UpdateEvent(@event);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            return View(@event);
        }

        // GET: Events/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _eventsService.GetEventById((int) id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var @event = _eventsService.GetEventById(id);

            if (@event != null)
            {
                _eventsService.DeleteEvent(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _eventsService.GetEventById(id) != null;
        }
    }
}
