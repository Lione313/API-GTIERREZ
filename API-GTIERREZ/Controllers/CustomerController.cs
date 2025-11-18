using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_GTIERREZ.Models;
using API_GTIERREZ.Request;
using API_GTIERREZ.Response;
using System.Collections.Generic;
using System.Linq;

namespace API_GTIERREZ.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly StoreContext _context;

        public CustomerController(StoreContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public IEnumerable<CustomerResponse> GetAll()
        {
            var customers = _context.Customers
                .Where(c => c.Active) 
                .ToList();

            var response = customers.Select(c => new CustomerResponse
            {
                CustomerID = c.CustomerID,
                FirstName = c.FirstName,
                LastName = c.LastName,
                DocumentNumber = c.DocumentNumber
            }).ToList();

            return response;
        }

        
        [HttpGet]
        public IEnumerable<CustomerResponse> Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return new List<CustomerResponse>();

            term = term.ToLower();

            var customers = _context.Customers
                .Where(c => c.Active &&
                            (c.FirstName.ToLower().Contains(term) ||
                             c.LastName.ToLower().Contains(term) ||
                             c.DocumentNumber.Contains(term)))
                .OrderByDescending(c => c.LastName)
                .ToList();

            var response = customers.Select(c => new CustomerResponse
            {
                CustomerID = c.CustomerID,
                FirstName = c.FirstName,
                LastName = c.LastName,
                DocumentNumber = c.DocumentNumber
            }).ToList();

            return response;
        }




        [HttpGet]
        public CustomerResponse GetById(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerID == id && c.Active);

            if (customer == null) return null;

            return new CustomerResponse
            {
                CustomerID = customer.CustomerID,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                DocumentNumber = customer.DocumentNumber
            };
        }

        
        [HttpGet]
        public IEnumerable<CustomerResponse> GetByName(string name)
        {
            var customers = _context.Customers
                .Where(c => c.Active && (c.FirstName.Contains(name) || c.LastName.Contains(name)))
                .ToList();

            return customers.Select(c => new CustomerResponse
            {
                CustomerID = c.CustomerID,
                FirstName = c.FirstName,
                LastName = c.LastName,
                DocumentNumber = c.DocumentNumber
            }).ToList();
        }


        [HttpPost]
        public string Insert(CustomerRequestInsert request)
        {
            try
            {
                var customer = new Customer
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DocumentNumber = request.DocumentNumber,
                    Active = true
                };

                _context.Customers.Add(customer);
                _context.SaveChanges();

                return "Cliente registrado exitosamente";
            }
            catch (Exception)
            {
                return "Error, comunicarse con el administrador";
            }
        }

     
        [HttpPut]
        public string Update(CustomerRequestUpdate request)
        {
            try
            {
                var customer = _context.Customers.FirstOrDefault(c => c.CustomerID == request.CustomerID && c.Active);

                if (customer == null) return "Cliente no encontrado";

                customer.FirstName = request.FirstName;
                customer.LastName = request.LastName;
                customer.DocumentNumber = request.DocumentNumber;

                _context.SaveChanges();

                return "Cliente actualizado exitosamente";
            }
            catch (Exception)
            {
                return "Error, comunicarse con el administrador";
            }
        }

       
        [HttpDelete]
        public string Delete(int id)
        {
            try
            {
                var customer = _context.Customers.FirstOrDefault(c => c.CustomerID == id && c.Active);

                if (customer == null) return "Cliente no encontrado";

                
                customer.Active = false;
                _context.SaveChanges();

                return "Cliente eliminado correctamente";
            }
            catch (Exception)
            {
                return "Error, comunicarse con el administrador";
            }
        }
    }
}
