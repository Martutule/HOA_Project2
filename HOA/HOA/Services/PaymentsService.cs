﻿using HOA.Models;
using HOA.Repositories.Interfaces;
using HOA.Services.Interfaces;

namespace HOA.Services
{
    public class PaymentsService: IPaymentsService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public PaymentsService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public Payment GetPaymentsById(int id)
        {
            return _repositoryWrapper.PaymentsRepository.FindByCondition(p => p.Id == id).FirstOrDefault();
        }
        public void AddPayment(Payment payment)
        {
            _repositoryWrapper.PaymentsRepository.Create(payment);
            _repositoryWrapper.Save();
        }
        public void UpdatePayment(Payment payment)
        {
            _repositoryWrapper.PaymentsRepository.Update(payment);
            _repositoryWrapper.Save();
        }
        public void DeletePayment(int id)
        {
            var payment = _repositoryWrapper.PaymentsRepository.FindByCondition(p => p.Id == id).FirstOrDefault();

            if (payment != null)
            {
                _repositoryWrapper.PaymentsRepository.Delete(payment);
                _repositoryWrapper.Save();
            }
        }
        public IEnumerable<Payment> GetAllPayments()
        {
            return _repositoryWrapper.PaymentsRepository.FindAll();
        }
        public IEnumerable<Payment> SearchPaymentsByResidentName(string name)
        {
            return _repositoryWrapper.PaymentsRepository.FindByCondition(p => p.ResidentName.ToLower().Contains(name.ToLower().Trim()));
        }
    }
}
