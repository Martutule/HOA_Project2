using HOA.Models;

namespace HOA.Services.Interfaces
{
    public interface IMaintenanceService
    {
        Maintenance GetMaintenanceById(int id);
        void AddMaintenance(Maintenance maintenance);
        void UpdateMaintenance(Maintenance maintenance);
        void DeleteMaintenance(int id);
        IEnumerable<Maintenance> GetAllMaintenance();
        IEnumerable<Maintenance> SearchMaintenanceByTaskName(string name);
    }
}
