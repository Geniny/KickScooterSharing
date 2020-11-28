using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KickScooterSharing.Models;
using KickScooterSharing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KickScooterSharing.Models;
using Microsoft.AspNetCore.Identity;

namespace KickScooterSharing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; 
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            this._context = context;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OrderMenu(int id)
        {
            var product = await this._context.Products.FirstOrDefaultAsync(x => x.Id == id);
            return PartialView(product);
        }

        [AllowAnonymous]
        public JsonResult GetProductLocations()
        {
            var locations = this._context.ProductLocations.ToList();
            return Json(locations);
        }

        [AllowAnonymous]
        public JsonResult GetParkingLocations()
        {
            var locations = this._context.parkingLocations.ToList();
            return Json(locations);
        }

        [Authorize]
        public async Task<IActionResult> OrderProduct(int id)
        {
            var product = await this._context.Products.FirstOrDefaultAsync(x => x.Id == id);
            var user = await this._userManager.GetUserAsync(this.User);
            product.Name = user.RegisterDate.ToString();

            return PartialView(product);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
