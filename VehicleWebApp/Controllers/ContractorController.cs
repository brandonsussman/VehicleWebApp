using Microsoft.AspNetCore.Mvc;
using VehicleWebApp.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration; // Add this using directive

namespace VehicleWebApp.Controllers
{
    public class ContractorController : Controller
    {
        private readonly IConfiguration _configuration;

        public ContractorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       
        public IActionResult CreateContractor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateContractor(Contractor contractor)
        {
            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    var sql = "INSERT INTO Contractor (ContractorId, Name, Email, PhoneNumber) VALUES (@ContractorId, @Name, @Email, @PhoneNumber)";

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ContractorId", contractor.ContractorId);
                        command.Parameters.AddWithValue("@Name", contractor.Name);
                        command.Parameters.AddWithValue("@Email", contractor.Email);
                        command.Parameters.AddWithValue("@PhoneNumber", contractor.PhoneNumber);

                        command.ExecuteNonQuery();
                    }
                }
                TempData["SuccessMessage"] = "Insert completed successfully.";
            }

            return View(contractor);
        }
    }
}
