using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.IO;
using System.Collections.Generic;

namespace WEB_CW.Pages.Admin.ManageWeb
{
    public class ManageImagesModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public ManageImagesModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // สำหรับดึงข้อมูลภาพ
        public List<ImageViewModel> ImageList { get; set; }


        public void OnGet()
        {
            // ดึงภาพจากฐานข้อมูล
            ImageList = GetImagesFromDatabase();
        }

        public async Task<IActionResult> OnPostUploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please upload a valid file.");
                return Page();
            }

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                string connString = _configuration.GetConnectionString("DefaultConnection");
                using (var connection = new MySqlConnection(connString))
                {
                    await connection.OpenAsync();

                    var command = new MySqlCommand("INSERT INTO ImagesSlideHome (Name, ImageData) VALUES (@Name, @ImageData)", connection);
                    command.Parameters.AddWithValue("@Name", file.FileName);
                    command.Parameters.AddWithValue("@ImageData", imageData);

                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToPage("/Admin/ManageWeb");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }


        // ดึงข้อมูลภาพจากฐานข้อมูล
        private List<ImageViewModel> GetImagesFromDatabase()
        {
            List<ImageViewModel> images = new List<ImageViewModel>();

            string connString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new MySqlConnection(connString))
            {
                connection.Open();

                var command = new MySqlCommand("SELECT Id, Name, ImageData FROM ImagesSlideHome", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var imageId = reader.GetInt32("Id");
                        var imageName = reader.GetString("Name");
                        var imageData = reader["ImageData"] as byte[];

                        if (imageData != null)
                        {
                            var base64Image = Convert.ToBase64String(imageData);
                            images.Add(new ImageViewModel
                            {
                                Id = imageId,
                                Name = imageName,
                                Base64Image = base64Image
                            });
                        }
                    }
                }
            }

            return images;
        }

        // ฟังก์ชันลบภาพจากฐานข้อมูล
        public async Task<IActionResult> OnPostDeleteImage(int imageId)
        {
            string connString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new MySqlConnection(connString))
            {
                await connection.OpenAsync();

                var command = new MySqlCommand("DELETE FROM ImagesSlideHome WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", imageId);

                await command.ExecuteNonQueryAsync();
            }

            return RedirectToPage();  // รีเฟรชหน้า
        }


    }

    public class ImageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Base64Image { get; set; }
    }

}
