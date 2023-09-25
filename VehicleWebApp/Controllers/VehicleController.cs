using Microsoft.AspNetCore.Mvc;
using VehicleWebApp.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace VehicleWebApp.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IConfiguration _configuration;

        public VehicleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult CreateVehicle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateVehicle(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    var sql = "INSERT INTO Vehicle (RegistrationNumber, Type, Model, Weight, ContractorID) " +
                              "VALUES (@RegistrationNumber, @Type, @Model, @Weight, @ContractorID)";

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@RegistrationNumber", vehicle.RegistrationNumber);
                        command.Parameters.AddWithValue("@Type", vehicle.Type);
                        command.Parameters.AddWithValue("@Model", vehicle.Model);
                        command.Parameters.AddWithValue("@Weight", vehicle.Weight);
                        command.Parameters.AddWithValue("@ContractorID", vehicle.ContractorID);
                        command.ExecuteNonQuery();
                    }
                }

            }

            return View(vehicle);
        }

        public IActionResult EditDelete(string registrationNumber)
        {
            Vehicle vehicle = GetVehicleByRegistrationNumber(registrationNumber);

            if (vehicle == null)
            {
                return NotFound();
            }

            return View("EditDelete", vehicle);
        }

        [HttpPost]
        public IActionResult Edit(string registrationNumber, Vehicle vehicle)
        {
            if (registrationNumber != vehicle.RegistrationNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                UpdateVehicleInDatabase(vehicle);
            }

            return View("EditDelete", vehicle);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string registrationNumber)
        {
            DeleteVehicleFromDatabase(registrationNumber);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            // Retrieve a list of all vehicles from the database
            List<Vehicle> vehicles = GetAllVehicles();

            return View(vehicles);
        }

        private List<Vehicle> GetAllVehicles()
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT RegistrationNumber, Type, Model, Weight, ContractorID FROM Vehicle";

                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var vehicle = new Vehicle
                            {
                                RegistrationNumber = reader.GetString("RegistrationNumber"),
                                Type = reader.GetString("Type"),
                                Model = reader.GetString("Model"),
                                Weight = reader.GetDecimal("Weight"),
                                ContractorID = reader.GetString("ContractorID")
                            };
                            vehicles.Add(vehicle);
                        }
                    }
                }
            }

            return vehicles;
        }

        private Vehicle GetVehicleByRegistrationNumber(string registrationNumber)
        {
            Vehicle vehicle = null;

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT RegistrationNumber, Type, Model, Weight, ContractorID FROM Vehicle WHERE RegistrationNumber = @RegistrationNumber";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@RegistrationNumber", registrationNumber);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            vehicle = new Vehicle
                            {
                                RegistrationNumber = reader.GetString("RegistrationNumber"),
                                Type = reader.GetString("Type"),
                                Model = reader.GetString("Model"),
                                Weight = reader.GetDecimal("Weight"),
                                ContractorID = reader.GetString("ContractorID")
                            };
                        }
                    }
                }
            }

            return vehicle;
        }

        private void UpdateVehicleInDatabase(Vehicle vehicle)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "UPDATE Vehicle SET Type = @Type, Model = @Model, Weight = @Weight, ContractorID = @ContractorID WHERE RegistrationNumber = @RegistrationNumber";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@RegistrationNumber", vehicle.RegistrationNumber);
                    command.Parameters.AddWithValue("@Type", vehicle.Type);
                    command.Parameters.AddWithValue("@Model", vehicle.Model);
                    command.Parameters.AddWithValue("@Weight", vehicle.Weight);
                    command.Parameters.AddWithValue("@ContractorID", vehicle.ContractorID);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteVehicleFromDatabase(string registrationNumber)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "DELETE FROM Vehicle WHERE RegistrationNumber = @RegistrationNumber";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@RegistrationNumber", registrationNumber);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
