using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KickScooterSharing.Data;
using KickScooterSharing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KickScooterSharing.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;


        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Display(Name = "Second name")]
            public string SecondName { get; set; }

            [Display(Name = "Balance")]
            public double Balance { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            user = await _context.Users
                .Include(u => u.FirstName)
                .Include(u => u.SecondName)
                .Where(u => u.Id == user.Id)
                .FirstAsync();
            var userName = user.UserName;
            var phoneNumber = user.PhoneNumber;
            string firstName = user.FirstName.Name ?? "";
            string secondName = user.SecondName.Name ?? "";
            double balance = user.Balance;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                SecondName = secondName,
                Balance = balance
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            FirstName firstName = await _context.FirstName.Where(o => o.Name == Input.FirstName).FirstOrDefaultAsync();
            if (firstName is null)
            {
                firstName = new FirstName() { Name = Input.FirstName };
                await _context.FirstName.AddAsync(firstName);
            }

            SecondName secondName = await _context.SecondNames.Where(o => o.Name == Input.SecondName).FirstOrDefaultAsync();
            if (secondName is null)
            {
                secondName = new SecondName() { Name = Input.SecondName };
                await _context.SecondNames.AddAsync(secondName);
            }

            user.FirstName = firstName;
            user.SecondName = secondName;
            user.Balance = Input.Balance;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();


            //if

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
