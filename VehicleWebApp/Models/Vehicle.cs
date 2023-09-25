namespace VehicleWebApp.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Vehicle
    {
        [Key]
        [Required]
        public string RegistrationNumber { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public decimal Weight { get; set; }


        [Required]
        public string ContractorID{ get; set; }
    }
}
