using HOA.Models;
using HOA.Repositories.Interfaces;

public class ResidentsService : IResidentsService
{
    private IRepositoryWrapper _repositoryWrapper;

    public ResidentsService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public IEnumerable<Resident> GetAllResidents()
    {
        return _repositoryWrapper.ResidentsRepository.FindAll();
    }

    public IEnumerable<Resident> SearchResidentsByName(string name)
    {
        return _repositoryWrapper.ResidentsRepository.FindByCondition(r => r.Name.ToLower().Contains(name.ToLower().Trim()));
    }

    public Resident GetResidentById(int id)
    {
        return _repositoryWrapper.ResidentsRepository.FindByCondition(r => r.Id == id).FirstOrDefault();
    }

    public void AddResident(Resident resident)
    {
        _repositoryWrapper.ResidentsRepository.Create(resident);
        _repositoryWrapper.Save();
    }

    public void UpdateResident(Resident resident)
    {
        _repositoryWrapper.ResidentsRepository.Update(resident);
        _repositoryWrapper.Save();
    }

    public void DeleteResident(int id)
    {
        var resident = _repositoryWrapper.ResidentsRepository.FindByCondition(r => r.Id == id).FirstOrDefault();
        if (resident != null)
        {
            _repositoryWrapper.ResidentsRepository.Delete(resident);
            _repositoryWrapper.Save();
        }
    }

}
