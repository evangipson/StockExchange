using System.Xml.Serialization;

namespace StockExchange.Domain.Models
{
	[XmlRoot("Companies")]
	public class CompanyList
	{
		private List<Company> _companies = new List<Company>();

		[XmlElement("Company")]
		public List<Company> Companies
		{
			get => _companies ?? (_companies = new List<Company>());
		}
	}
}
