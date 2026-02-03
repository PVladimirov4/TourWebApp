using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TourWebApp.Core.Contracts;
using TourWebApp.Infrastructure.Data;
using TourWebApp.Infrastructure.Data.Entities;

namespace TourWebApp.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;

        public OrderService(ApplicationDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }
        public bool Create(int productId, string userId, int quantity) 
        {
            var product = this._context.Products.SingleOrDefault(x => x.Id == productId);

            if (product == null)
            {
                return false;
            }
            Order item = new Order
            {
                OrderDate = DateTime.Now,
                ProductId = productId,
                UserId = userId,
                Quantity = quantity,
                Price = product.Price,
                Discount = product.Discount
            };

            product.Quantity -= quantity;

            this._context.Products.Update(product);
            this._context.Orders.Add(item);

            return _context.SaveChanges() != 0;
        }
    }
}
