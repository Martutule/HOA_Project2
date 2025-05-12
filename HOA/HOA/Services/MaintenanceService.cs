using HOA.Models;
using HOA.Repositories.Interfaces;
using HOA.Services.Interfaces;

namespace HOA.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public MaintenanceService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public Maintenance GetMaintenanceById(int id)
        {
            return _repositoryWrapper.MaintenanceRepository.FindByCondition(a => a.Id == id).FirstOrDefault();

        }
        public void AddMaintenance(Maintenance maintenance)
        {
            _repositoryWrapper.MaintenanceRepository.Create(maintenance);
            _repositoryWrapper.Save();
        }
        public void UpdateMaintenance(Maintenance maintenance)
        {
            _repositoryWrapper.MaintenanceRepository.Update(maintenance);
            _repositoryWrapper.Save();
        }
        public void DeleteMaintenance(int id)
        {
            var maintenance = _repositoryWrapper.MaintenanceRepository.FindByCondition(p => p.Id == id).FirstOrDefault();

            if (maintenance != null)
            {
                _repositoryWrapper.MaintenanceRepository.Delete(maintenance);
                _repositoryWrapper.Save();
            }
        }
        public IEnumerable<Maintenance> GetAllMaintenance()
        {
            return _repositoryWrapper.MaintenanceRepository.FindAll();
        }
        public IEnumerable<Maintenance> SearchMaintenanceByTaskName(string name)
        {
            return _repositoryWrapper.MaintenanceRepository.FindByCondition(p => p.TaskName.ToLower().Contains(name.ToLower().Trim()));
        }
    }
}
