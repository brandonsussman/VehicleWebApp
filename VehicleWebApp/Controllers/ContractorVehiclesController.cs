using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VehicleWebApp.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;


namespace VehicleWebApp.Controllers
{
    public class ContractorVehiclesController : Controller
    {
        private readonly IConfiguration _configuration;

        public ContractorVehiclesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       
        public IActionResult ViewContractorVehicles(string selectedContractorId)
        {
            List<Contractor> contractors = GetContractors();

            var viewModel = new ContractorVehicles
            {
                ContractorList = contractors,
                SelectedContractorId = selectedContractorId
            };

            if (!string.IsNullOrEmpty(selectedContractorId))
            {
                List<Vehicle> contractorVehicles = GetVehiclesForContractor(selectedContractorId);

                viewModel.SelectedContractorName = GetContractorName(selectedContractorId);
                viewModel.VehicleList = contractorVehicles;
            }

            return View(viewModel);
        }

        private List<Contractor> GetContractors()
        {
            List<Contractor> contractors = new List<Contractor>();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Contractor";

                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contractor = new Contractor
                            {
                                ContractorId = reader.GetString("ContractorId"),
                                Name = reader.GetString("Name"),
                                Email = reader.GetString("Email"),
                                PhoneNumber = reader.GetString("PhoneNumber")
                            };
                            contractors.Add(contractor);
                        }
                    }
                }
            }

            return contractors;
        }

        private List<Vehicle> GetVehiclesForContractor(string contractorId)
        {
            List<Vehicle> contractorVehicles = new List<Vehicle>();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Vehicle WHERE ContractorId = @ContractorId";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ContractorId", contractorId);

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
                            contractorVehicles.Add(vehicle);
                        }
                    }
                }
            }

            return contractorVehicles;
        }

        private string GetContractorName(string contractorId)
        {
            string contractorName = "";

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT Name FROM Contractor WHERE ContractorId = @ContractorId";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ContractorId", contractorId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contractorName = reader.GetString("Name");
                        }
                    }
                }
            }

            return contractorName;
        }
    }
}
