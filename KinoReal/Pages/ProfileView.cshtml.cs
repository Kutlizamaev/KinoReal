using KinoReal.DB;
using KinoReal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KinoReal.Pages
{
    public class ProfileViewModel : PageModel
    {
        public Profile profile { get; set; }
        public async Task<ActionResult> OnGet()
        {
            if (HttpContext.Session.Keys.Contains("Id"))
            { 
                var id = HttpContext.Session.GetString("Id");
                var profiles = await MyDb.GetAllProfiles();
                profile = profiles.FirstOrDefault(p => p.Id.ToString() == id);

                return Page();
            }
            else
            {
                return RedirectToPage("/Login");
            }
        }
        public async Task<ActionResult> OnPost(string fullname, string city, string birthday)
        {
            Profile currentProfile = new Profile()
            {
                Id = Guid.Parse(HttpContext.Session.GetString("Id")),
                Fullname = fullname,
                City = city,
                Birthday = birthday
            };

            await MyDb.Update(currentProfile);

            return RedirectToPage("/ProfileView");
        }
    }
}
