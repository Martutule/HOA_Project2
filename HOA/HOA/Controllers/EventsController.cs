using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HOA.Models;
using HOA.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HOA.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private IEventsService _eventsService;
        private readonly HOADbContext _context;

        public EventsController(IEventsService eventsService, HOADbContext context)
        {
            _eventsService = eventsService;
            _context = context;
        }

        // GET: Events
        public IActionResult Index(string searchQuery)
        {
            ViewData["SearchQuery"] = searchQuery;
            var events = string.IsNullOrEmpty(searchQuery)
                ? _eventsService.GetAllEvents()
                : _eventsService.SearchEventsByEventName(searchQuery);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var joinedEvents = _context.EventParticipants
                .Where(ep => ep.UserId == userId)
                .Select(ep => ep.EventId)
                .ToHashSet();
            ViewBag.JoinedEvents = joinedEvents;
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

        [Authorize(Roles = "Admin")]
        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }
        
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleJoin(int eventId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var participant = await _context.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId);

            bool joined;
            if (participant == null)
            {
                _context.EventParticipants.Add(new EventParticipant { EventId = eventId, UserId = userId });
                joined = true;
            }
            else
            {
                _context.EventParticipants.Remove(participant);
                joined = false;
            }
            await _context.SaveChangesAsync();
            return Json(new { joined });
        }

        [HttpGet]
        public async Task<IActionResult> Participants(int id)
        {
            var @event = _eventsService.GetEventById(id);
            if (@event == null)
                return NotFound();

            var participants = await _context.EventParticipants
                .Where(ep => ep.EventId == id)
                .ToListAsync();

            // Get user information separately
            var userIds = participants.Select(p => p.UserId).ToList();
            var userList = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            var users = userList.ToDictionary(u => u.Id, u => new { u.UserName, u.Email });

            // Debug information - remove this after fixing
            Console.WriteLine($"Participant UserIds: {string.Join(", ", userIds)}");
            Console.WriteLine($"Found Users: {string.Join(", ", users.Keys)}");
            Console.WriteLine($"Users count: {users.Count}, Participants count: {participants.Count}");

            ViewBag.Users = users;
            ViewBag.EventName = @event.EventName;
            return View(participants);
        }


    }
}
