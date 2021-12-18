using KinoReal.DB;
using KinoReal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace KinoReal.Pages
{
    public class LoginModel : PageModel
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

        public async Task<ActionResult> OnPost(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var users = await MyDb.GetAllUsers();
                var user = users.FirstOrDefault(u => (u.Username == username || u.Email == username)&&(Encryption.DecryptString(u.Password) == password));
                if (user != null)
                { 
                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("Id", user.Id.ToString());
                }
                else
                {
                    ModelState.AddModelError("", "Email or password entered incorrectly");
                }
            }
            return RedirectToPage("/Login");
        }
    }
}
