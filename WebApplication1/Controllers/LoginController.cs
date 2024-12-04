using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient; 
using WebApplication1.Models;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT password FROM users WHERE name = @Username";
                using (MySqlCommand command = new MySqlCommand(query, connection))  
                {
                    command.Parameters.AddWithValue("@Username", username);

                    string? storedPasswordHash = command.ExecuteScalar()?.ToString();

                    if (storedPasswordHash != null && storedPasswordHash == password)
                    {
                        HttpContext.Session.SetString("UserLoggedIn", "true");
                        HttpContext.Session.SetString("Username", username);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Error = "Invalid username or password!";
                        return View();
                    }
                }
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserLoggedIn");
            HttpContext.Session.Remove("Username");

            return RedirectToAction("Index");
        }
    }
}
