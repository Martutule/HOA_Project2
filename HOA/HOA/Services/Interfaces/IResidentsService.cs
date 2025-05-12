using HOA.Models;

public interface IResidentsService
{
    Resident GetResidentById(int id);
    void AddResident(Resident resident);
    void UpdateResident(Resident resident);
    void DeleteResident(int id);
    IEnumerable<Resident> GetAllResidents();
    IEnumerable<Resident> SearchResidentsByName(string name);
}
