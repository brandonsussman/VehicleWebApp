using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using VehicleWebApp.Models;

namespace VehicleWebApp.Controllers
{
    public class SummaryController : Controller
    {
        private readonly IConfiguration _configuration;

        public SummaryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult ViewSummary()
        {
            List<Summary> summaryList = GetContractorSummary();

            return View(summaryList);
        }

        private List<Summary> GetContractorSummary()
        {
            List<Summary> summaryList = new List<Summary>();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT c.ContractorId, c.Name AS ContractorName, COUNT(v.RegistrationNumber) AS NumberOfVehicles, SUM(v.Weight) AS TotalWeight
                    FROM Contractor c
                    LEFT JOIN Vehicle v ON c.ContractorId = v.ContractorID
                    GROUP BY c.ContractorId, c.Name;";

                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var summary = new Summary
                            {
                                ContractorId = reader.GetString("ContractorId"),
                                ContractorName = reader.GetString("ContractorName"),
                                NumberOfVehicles = reader.GetInt32("NumberOfVehicles"),
                                
                                TotalWeight = reader.GetDouble("TotalWeight")
                            };
                            summaryList.Add(summary);
                        }
                    }
                }
            }

            return summaryList;
        }
    }
}
