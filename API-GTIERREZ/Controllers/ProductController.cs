using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_GTIERREZ.Models;
using API_GTIERREZ.Request;
using API_GTIERREZ.Response;

namespace API_GTIERREZ.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly StoreContext _context;

        public ProductController(StoreContext context)
        {
            _context = context;
        }

    
        [HttpGet]
        public IEnumerable<ProductResponse> GetAll()
        {
            var products = _context.Products
                .Where(p => p.Active) 
                .ToList();

            var response = products.Select(p => new ProductResponse
            {
                ProductID = p.ProductID,
                Name = p.Name,
                Price = p.Price
            }).ToList();

            return response;
        }

   
        [HttpGet]
        public ProductResponse GetById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == id && p.Active);

            if (product == null) return null;

            return new ProductResponse
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Price = product.Price
            };
        }

      
        [HttpGet]
        public IEnumerable<ProductResponse> GetByName(string name)
        {
            var products = _context.Products
                .Where(p => p.Active && p.Name.Contains(name))
                .ToList();

            return products.Select(p => new ProductResponse
            {
                ProductID = p.ProductID,
                Name = p.Name,
                Price = p.Price
            }).ToList();
        }

      
        [HttpPost]
        public string Insert(ProductRequestInsert request)
        {
            try
            {
                var product = new Product
                {
                    Name = request.Name,
                    Price = request.Price,
                    Active = true
                };

                _context.Products.Add(product);
                _context.SaveChanges();

                return "Producto registrado exitosamente";
            }
            catch (Exception)
            {
                return "Error, comunicarse con el administrador";
            }
        }

       
        [HttpPut]
        public string Update(ProductRequestUpdate request)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductID == request.ProductID && p.Active);

                if (product == null) return "Producto no encontrado";

                product.Name = request.Name;
                product.Price = request.Price;

                _context.SaveChanges();

                return "Producto actualizado exitosamente";
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
                var product = _context.Products.FirstOrDefault(p => p.ProductID == id && p.Active);

                if (product == null) return "Producto no encontrado";

                
                product.Active = false;
                _context.SaveChanges();

                return "Producto eliminado correctamente";
            }
            catch (Exception)
            {
                return "Error, comunicarse con el administrador";
            }
        }
    }
}
