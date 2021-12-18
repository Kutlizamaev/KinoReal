using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinoReal.Pages
{
    public class MovieViewModel : PageModel
    {
        public string movieName { get; set; }
        public void OnGet(string name)
        {
            movieName = name + ".mp4";
        }
        public void OnPost()
        {

        }
    }
}
