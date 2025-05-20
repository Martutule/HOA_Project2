using HOA.Models;
using HOA.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HOA.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            Dashboard dashboard = _dashboardService.GetDashboardData();
            return View(dashboard);
        }
    }

}
