using StockExchange.Domain.Models;

namespace StockExchange.Logic.Services.Interfaces
{
	public interface ISerializationService
	{
		IEnumerable<Company> GetCompany(string? companyName = null);

		bool SetCompany(Company company);
	}
}
