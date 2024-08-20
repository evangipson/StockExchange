using StockExchange.Base.Serialization.Repositories.Interfaces;
using StockExchange.Domain.Models;

namespace StockExchange.Logic.Repositories.Interfaces
{
    public interface ICompanyRepository : ISerializableRepository<Company>
    {
    }
}
