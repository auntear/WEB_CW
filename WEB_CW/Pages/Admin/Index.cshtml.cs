using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace WEB_CW.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public void OnGet()
        {
        }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnPost()
        {
            string username = Request.Form["Username"];
            string password = Request.Form["Password"];

            string connString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new MySqlConnection(connString))
            {               
                string query = "SELECT COUNT(1) FROM users WHERE username = @Username AND password = @Password";
                connection.Open();

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    var result = Convert.ToInt32(cmd.ExecuteScalar());

                    if (result > 0)
                    {
                        // หากพบข้อมูล ให้ทำการ Redirect ไปที่หน้าถัดไป (เช่น หน้า Dashboard)
                        // การเก็บข้อความใน Session
                        HttpContext.Session.SetString("Message", username);
                        return RedirectToPage("/Admin/ManageWeb");
                    }
                    return Page();
                }

            }
        }

    }
}
