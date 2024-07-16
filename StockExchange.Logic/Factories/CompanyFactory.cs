using StockExchange.Domain.Models;
using StockExchange.Logic.Factories.Interfaces;

namespace StockExchange.Logic.Factories
{
	public class CompanyFactory : ICompanyFactory
	{
		public Company MakeCompany()
		{
			return new Company();
		}
	}
}
