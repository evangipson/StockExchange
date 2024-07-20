using StockExchange.Domain.Models;

namespace StockExchange.Logic.Repositories.Interfaces
{
	public interface ICompanyRepository
	{
		IEnumerable<Company>? GetCompany(string? companyName = null);

		bool UpdateCompany(Company company);
	}
}
