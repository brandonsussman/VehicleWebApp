namespace VehicleWebApp.Models
{
    internal class ContractorVehicles
    {
        public List<Contractor> ContractorList { get; set; }
        
        public List<Vehicle> VehicleList { get; set; }
        public string SelectedContractorId { get; set; }
        public string SelectedContractorName { get; set; }
    }
}