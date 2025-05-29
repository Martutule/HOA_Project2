using HOA.Models;
using HOA.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HOA.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: Notifications
        public IActionResult Index()
        {
            var notifications = _notificationService.GetAllNotifications();
            return View(notifications);
        }

        // GET: Notifications/Details/5
        public IActionResult Details(int id)
        {
            var notification = _notificationService.GetNotificationById(id);
            if (notification == null)
                return NotFound();

            return View(notification);
        }

        // GET: Notifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notifications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Notification notification)
        {
            if (ModelState.IsValid)
            {
                notification.Timestamp = DateTime.Now;
                _notificationService.CreateNotification(notification);
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }

        // GET: Notifications/Edit/5
        public IActionResult Edit(int id)
        {
            var notification = _notificationService.GetNotificationById(id);
            if (notification == null)
                return NotFound();

            return View(notification);
        }

        // POST: Notifications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Notification notification)
        {
            if (ModelState.IsValid)
            {
                _notificationService.UpdateNotification(notification);
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }


        // Used for deleting directly via trash icon
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _notificationService.DeleteNotification(id);
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}
