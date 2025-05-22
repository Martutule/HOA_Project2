// HOA/Services/Interfaces/ISupplierContractService.cs
using System.Collections.Generic;
using HOA.Models;

public interface ISupplierContractService
{
    IEnumerable<SupplierContract> GetAllContracts();
    SupplierContract GetContractById(int id);
    void AddContract(SupplierContract contract);
    void UpdateContract(SupplierContract contract);
    void DeleteContract(int id);
}
