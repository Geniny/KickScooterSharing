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

        [AllowAnonymous]
        public async Task<IActionResult> OrderMenu(int id)
        {
            var product = await this._context.Products.Include(p => p.Tariff).Include(p => p.Scooter).ThenInclude(x => x.ScooterModel).FirstOrDefaultAsync(x => x.Id == id);
            Console.WriteLine(id);
            Debug.WriteLine(id);
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
            var locations = this._context.ParkingLocations.ToList();
            return Json(locations);
        }

        [Authorize]
        public async Task<IActionResult> OrderProduct(int id, int minutes)
        {
            var product = await this._context.Products.Include(p => p.Tariff).FirstOrDefaultAsync(x => x.Id == id);
            var user = await this._userManager.GetUserAsync(this.User);

            if (product.Status.Name  == "free")
            {
                if (user.Balance - (product.Tariff.StartPrice + product.Tariff.CostPerMinute * minutes) <= 0)
                {
                    ModelState.AddModelError(string.Empty, $"Not enough money");
                    return PartialView(null);
                }
                else
                {
                    product.StatusId = 2;
                    DateTime currentTime = DateTime.Now.AddSeconds(15);
                    DateTime endTime = currentTime.AddMinutes(minutes);
                    var sale = new Sales() { User = user, Product = product, DateTime = DateTime.Now, EndDate = endTime, Price = minutes * product.Tariff.CostPerMinute + product.Tariff.StartPrice };
                    _context.Add(sale);
                    await _context.SaveChangesAsync();                   
                    return PartialView(sale);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"The {product.Name} is busy now");
                return PartialView(null);
            }
            
        }

        public async Task<IActionResult> BookProduct(int id)
        {
            return PartialView();
        }

        public async Task<IActionResult> EndOrdering(int id)
        {
            return PartialView();
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
