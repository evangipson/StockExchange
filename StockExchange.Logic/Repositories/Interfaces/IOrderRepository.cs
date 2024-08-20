using StockExchange.Base.Serialization.Repositories.Interfaces;
using StockExchange.Domain.Models.Orders;

namespace StockExchange.Logic.Repositories.Interfaces
{
    public interface IOrderRepository : ISerializableRepository<Order>
    {
    }
}
