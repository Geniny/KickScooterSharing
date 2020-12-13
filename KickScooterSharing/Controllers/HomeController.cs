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

        enum Status
        {
            free = 1,
            busy = 2
        }
         


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
            var product = await this._context.Products
                .Include(p => p.Tariff)
                .Include(p => p.Scooter).ThenInclude(x => x.ScooterModel)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(x => x.Id == id);
            return PartialView(product);
        }

        [AllowAnonymous]
        public JsonResult GetProductLocations()
        {
            var locations = this._context.ProductLocations
                .ToList();
            return Json(locations);
        }

        [AllowAnonymous]
        public JsonResult GetParkingLocations()
        {
            var locations = this._context.ParkingLocations.ToList();
            return Json(locations);
        }

        [Authorize]
        public async Task<IActionResult> OrderProduct(int id)
        {
            var product = await this._context.Products.Include(p => p.Tariff).Include(x => x.Status).FirstOrDefaultAsync(x => x.Id == id);
            var user = await this._userManager.GetUserAsync(this.User);

            if (product.Status.Id == (int)Status.busy)
            {
                ModelState.AddModelError(string.Empty, $"The {product.Name} is busy now");
                return PartialView(null);
            }

            if (user.Status.Id == (int)Status.busy)
            {
                ModelState.AddModelError(string.Empty, $"You ordered a product alerady.");
                return PartialView(null);
            }

            if (user.Balance - (product.Tariff.StartPrice + product.Tariff.CostPerMinute * 2) <= 0)
            {
                ModelState.AddModelError(string.Empty, $"You don't have enough money");
                return PartialView(null);
            }
            else
            {
                var sale = new Sales();

                product.StatusId = (int)Status.busy;
                user.StatusId = (int)Status.busy;
                user.Balance = user.Balance - product.Tariff.StartPrice;
                sale.Product = product;
                sale.User = user;
                sale.StartDate = DateTime.Now;

                _context.Update(product);
                _context.Update(user);
                _context.Add(sale);
                await _context.SaveChangesAsync();

                return PartialView(sale);
            }
        }

        [Authorize]
        public async Task<IActionResult> ReturnProduct(int id)
        {
            var product = await this._context.Products.Include(p => p.Tariff).Include(x => x.Status).FirstOrDefaultAsync(x => x.Id == id);
            var user = await this._userManager.GetUserAsync(this.User);
            var sale = await this._context.Sales.Where(s => s.ProductId == product.Id && s.UserId == user.Id).LastOrDefaultAsync();
            
             var parkingsLocation = await this._context.ParkingLocations
                .Where(location => Math.Abs(location.Latitude - location.Latitude) <= 0.0003 && Math.Abs(location.Longitude - location.Longitude) <= 0.0003)
                .ToListAsync();
            
            if (sale == null)
            {
                return PartialView(null);
            }
            
            if (parkingsLocation.Count >= 1)
            {
                var endDate = DateTime.Now;
                var price = (endDate - sale.StartDate).TotalMinutes * product.Tariff.CostPerMinute;
                sale.EndDate = endDate;
                sale.Price = price;
                user.Balance -= price;
                user.StatusId = (int)Status.free;
                product.StatusId = (int)Status.free;

                _context.Update(product);
                _context.Update(user);
                _context.Update(sale);
            }

            

            return null;
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
