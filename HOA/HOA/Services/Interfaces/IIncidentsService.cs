using HOA.Models;

namespace HOA.Services.Interfaces
{
    public interface IIncidentsService
    {
        Incident GetIncidentsById(int id);
        void AddIncident(Incident incident);
        void UpdateIncident(Incident incident);
        void DeleteIncident(int id);
        IEnumerable<Incident> GetAllIncidents();
        IEnumerable<Incident> SearchIncidentsByIncidentName(string name);
        public IEnumerable<Incident> SortIncidentsByDate(bool descending);
        void UpdateIncidentStatus(int id, string state);
    }
}
