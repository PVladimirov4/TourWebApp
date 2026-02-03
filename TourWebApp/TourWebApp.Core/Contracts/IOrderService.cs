using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TourWebApp.Infrastructure.Data.Entities;

namespace TourWebApp.Core.Contracts
{
    public interface IOrderService
    {
        bool Create(int productId, string userId, int quantity);

        List<Order> GetOrders();
    }
}
