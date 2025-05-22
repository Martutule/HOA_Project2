using System.Collections.Generic;
using System.Linq;
using HOA.Models;
using HOA.Repositories.Interfaces;

public class SupplierContractService : ISupplierContractService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public SupplierContractService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public IEnumerable<SupplierContract> GetAllContracts() =>
        _repositoryWrapper.SupplierContractRepository.FindAll().ToList();

    public SupplierContract GetContractById(int id) =>
        _repositoryWrapper.SupplierContractRepository
            .FindByCondition(c => c.Id == id)
            .FirstOrDefault();

    public void AddContract(SupplierContract contract)
    {
        _repositoryWrapper.SupplierContractRepository.Create(contract);
        _repositoryWrapper.Save();
    }

    public void UpdateContract(SupplierContract contract)
    {
        _repositoryWrapper.SupplierContractRepository.Update(contract);
        _repositoryWrapper.Save();
    }

    public void DeleteContract(int id)
    {
        var contract = GetContractById(id);
        if (contract != null)
        {
            _repositoryWrapper.SupplierContractRepository.Delete(contract);
            _repositoryWrapper.Save();
        }
    }
}
