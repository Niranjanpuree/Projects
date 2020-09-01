using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Core.Services
{
    public class PaystubService : IPaystubService
    {
        private readonly IPaystubRepository _repo;

        public PaystubService(IPaystubRepository repo)
        {
            _repo = repo;

        }
        public IEnumerable<Paystub> GetByEmployeeId()
        {
            var empId = "John";
            return _repo.GetByEmployeeId(empId);
        }
    }
}
