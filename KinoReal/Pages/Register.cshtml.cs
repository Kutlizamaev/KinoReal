using KinoReal.DB;
using KinoReal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace KinoReal.Pages.Shared
{
    public class RegisterModel : PageModel
    {
        public ActionResult OnGet()
        {
            if (HttpContext.Session.Keys.Contains("Id"))
            {
                return RedirectToPage("/ProfileView");
            }
            else
            {
                return Page();
            }
        }

        public async Task<ActionResult> OnPost(string email, string username, string password)
        {
            if (ModelState.IsValid && (email != null || username != null || password != null))
            {
                var users = await MyDb.GetAllUsers();
                var user = users.FirstOrDefault(u => u.Email == email || u.Username == username);
                if (user == null)
                {

                    var currentUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = email,
                        Password = Encryption.EncryptString(password),
                        Username = username
                    };

                    Profile profile = new Profile()
                    {
                        Id = currentUser.Id
                    };

                    await MyDb.Add(currentUser);
                    await MyDb.Add(profile);

                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("Id", currentUser.Id.ToString());
                }
                else
                {
                    ModelState.AddModelError("", "Data entered incorrectly");
                }
            }
            return RedirectToPage("/Register");
        }
    }
}
