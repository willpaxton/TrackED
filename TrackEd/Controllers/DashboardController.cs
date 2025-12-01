using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TrackEd.Models;

namespace TrackEd.Controllers {
    [Authorize]
    public class DashboardController : Controller {
        private readonly ILogger<HomeController> _logger;

        public DashboardController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            return View();
        }

    }
}


