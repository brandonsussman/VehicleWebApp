using System.ComponentModel.DataAnnotations;

namespace VehicleWebApp.Models
{

public class Contractor
    {
        [Key]
        
        public string ContractorId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

    }

}
