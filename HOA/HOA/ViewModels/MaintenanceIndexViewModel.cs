using System.Collections.Generic;
using HOA.Models;

namespace HOA.ViewModels
{
    public class MaintenanceIndexViewModel
    {
        public IEnumerable<Maintenance> Maintenances { get; set; }
        public IEnumerable<SupplierContract> SupplierContracts { get; set; }
    }
}
