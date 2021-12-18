using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinoReal.Pages
{
    public class LogoutModel : PageModel
    {
        public ActionResult OnGet()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/index");
        }
    }
}
