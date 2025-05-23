﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HOA.Models;
using HOA.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HOA.Repositories.Interfaces;

namespace HOA.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private IPaymentsService _paymentsService;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public PaymentsController(IPaymentsService paymentsService, IRepositoryWrapper repositoryWrapper)
        {
            _paymentsService = paymentsService;
            _repositoryWrapper = repositoryWrapper;
        }

        // GET: Payments
        public IActionResult Index(string searchQuery)
        {
            ViewData["SearchQuery"] = searchQuery;

            var payments = string.IsNullOrEmpty(searchQuery)
                ? _paymentsService.GetAllPayments()
                : _paymentsService.SearchPaymentsByApartmentNumber(searchQuery);

            return View(payments);
        }

        // GET: Payments/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var payment = _paymentsService.GetPaymentsById((int)id);
            
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        [HttpGet]
        public IActionResult PayNow(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var payment = _paymentsService.GetPaymentsById((int)id);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        [HttpPost]
        public IActionResult PayNowConfirmed(int id)
        {
            var payment = _paymentsService.GetPaymentsById(id);
            if (payment == null)
            {
                return NotFound();
            }
            
            _paymentsService.UpdatePaymentStatus(id, "Paid");

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        // GET: Payments/Create
        public IActionResult Create()
        {
            var apartments = _repositoryWrapper.ResidentsRepository
            .FindAll()
            .Select(r => r.Apartment)
            .Distinct()
            .OrderBy(a => a)
            .ToList();

            ViewBag.Apartments = apartments;
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,PaymentType,Apartment,PaymentDate,Amount,Status")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _paymentsService.AddPayment(payment);
                return RedirectToAction(nameof(Index));
            }
            // Repopulate dropdown on error
            var apartments = _repositoryWrapper.ResidentsRepository
                .FindAll()
                .Select(r => r.Apartment)
                .Distinct()
                .OrderBy(a => a)
                .ToList();
            ViewBag.Apartments = apartments;
            return View(payment);
        }

        [Authorize(Roles = "Admin")]
        // GET: Payments/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = _paymentsService.GetPaymentsById((int)id);

            if (payment == null)
            {
                return NotFound();
            }
            // Fetch apartments for dropdown
            var apartments = _repositoryWrapper.ResidentsRepository
                .FindAll()
                .Select(r => r.Apartment)
                .Distinct()
                .OrderBy(a => a)
                .ToList();
            ViewBag.Apartments = apartments;
            return View(payment);
        }

        [Authorize(Roles = "Admin")]
        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,PaymentType,Apartment,PaymentDate,Amount,Status")] Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _paymentsService.UpdatePayment(payment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdown if validation fails
            var apartments = _repositoryWrapper.ResidentsRepository
                .FindAll()
                .Select(r => r.Apartment)
                .Distinct()
                .OrderBy(a => a)
                .ToList();
            ViewBag.Apartments = apartments;

            return View(payment);

        }

        [Authorize(Roles = "Admin")]
        // GET: Payments/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = _paymentsService.GetPaymentsById((int)id);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        [Authorize(Roles = "Admin")]
        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var payment = _paymentsService.GetPaymentsById(id);

            if (payment != null)
            {
                _paymentsService.DeletePayment(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _paymentsService.GetPaymentsById(id) != null;
        }
    }
}
