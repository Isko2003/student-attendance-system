using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IConfiguration _configuration;

        public StudentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            List<StudentAttendance> students = GetStudentsFromDatabase();
            return View(students);
        }

        private List<StudentAttendance> GetStudentsFromDatabase()
        {
            List<StudentAttendance> students = new List<StudentAttendance>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, name, attendance_date, status FROM student_attendance";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new StudentAttendance
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                AttendanceDate = reader.GetDateTime("attendance_date"),
                                Status = reader.GetString("status")
                            });
                        }
                    }
                }
            }

            return students;
        }
    }
}
