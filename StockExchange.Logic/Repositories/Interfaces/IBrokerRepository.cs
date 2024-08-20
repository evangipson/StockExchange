using StockExchange.Base.Serialization.Repositories.Interfaces;
using StockExchange.Domain.Models.Actors;

namespace StockExchange.Logic.Repositories.Interfaces
{
    public interface IBrokerRepository : ISerializableRepository<Broker>
    {
    }
}
