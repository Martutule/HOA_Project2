using HOA.Models;
using HOA.Repositories.Interfaces;
using HOA.Services.Interfaces;

namespace HOA.Services
{
    public class IncidentsService : IIncidentsService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public IncidentsService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public Incident GetIncidentsById(int id)
        {
            return _repositoryWrapper.IncidentsRepository.FindByCondition(i => i.Id == id).FirstOrDefault();
        }
        public void AddIncident(Incident incident)
        {
            _repositoryWrapper.IncidentsRepository.Create(incident);
            _repositoryWrapper.Save();
        }
        public void UpdateIncident(Incident incident)
        {
            _repositoryWrapper.IncidentsRepository.Update(incident);
            _repositoryWrapper.Save();
        }
        public void DeleteIncident(int id)
        {
            var incident = _repositoryWrapper.IncidentsRepository.FindByCondition(i => i.Id == id).FirstOrDefault();

            if (incident != null)
            {
                _repositoryWrapper.IncidentsRepository.Delete(incident);
                _repositoryWrapper.Save();
            }
        }
        public IEnumerable<Incident> GetAllIncidents()
        {
            return _repositoryWrapper.IncidentsRepository.FindAll();
        }
        public IEnumerable<Incident> SearchIncidentsByIncidentName(string name)
        {
            return _repositoryWrapper.IncidentsRepository.FindByCondition(i => i.Title.ToLower().Contains(name.ToLower().Trim()));
        }

        public void UpdateIncidentStatus(int id, string state)
        {
            _repositoryWrapper.IncidentsRepository.UpdateIncidentStatus(id, state);
        }

        public IEnumerable<Incident> SortIncidentsByDate(bool descending = false)
        {
            var incidents = _repositoryWrapper.IncidentsRepository.FindAll();

            return descending
                ? incidents.OrderByDescending(i => i.Date)
                : incidents.OrderBy(i => i.Date);
        }

    }
}
