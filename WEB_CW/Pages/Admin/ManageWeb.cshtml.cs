using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WEB_CW.Pages.Admin
{
    public class ManageWebModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPostManageImages()
        {
            return RedirectToPage("/ManageImages");
        }

    }
}
