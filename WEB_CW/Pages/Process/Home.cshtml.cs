using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WEB_CW.Pages.Process
{
    public class HomeModel : PageModel
    {
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("/Process/Test");
        }
    }
}
