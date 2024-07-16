using System.Globalization;

namespace StockExchange.Domain.Models
{
	public class Company
	{
		public string Name { get; set; } = string.Empty;

		public string TickerName { get; set; } = string.Empty;

		public int StockAmount { get; set; }

		public DateTime IPODate { get; set; }

		public float CostOfGoodsSold { get; set; }

		public float NetSales { get; set; }

		public float Debt { get; set; }

		public float NonOperatingItems { get; set; }

		public List<float> Expenses { get; set; } = new List<float>();

		public Stock? Stock { get; set; }

		public float GrossProfit => NetSales - CostOfGoodsSold;

		public float OperatingExpenses => Expenses.Sum();

		public float OperatingIncome => GrossProfit - OperatingExpenses;

		public float NetIncome => OperatingIncome - NonOperatingItems;

		public override string ToString()
		{
			var companyData = new List<string>
			{
				"Company Information:",
				$"Name: {Name}",
				$"Ticker Name: {TickerName}",
				$"IPO Date: {IPODate}",
				$"Cost Of Goods Sold: {CostOfGoodsSold.ToString("C", CultureInfo.CurrentCulture)}",
				$"Net Sales: {NetSales.ToString("C", CultureInfo.CurrentCulture)}",
				$"Debt: {Debt.ToString("C", CultureInfo.CurrentCulture)}",
				$"Non Operating Items: {NonOperatingItems.ToString("C", CultureInfo.CurrentCulture)}",
				$"Gross Profit: {GrossProfit.ToString("C", CultureInfo.CurrentCulture)}",
				$"Operating Expenses: {OperatingExpenses.ToString("C", CultureInfo.CurrentCulture)}",
				$"Operating Income: {OperatingIncome.ToString("C", CultureInfo.CurrentCulture)}",
				$"Net Income: {NetIncome.ToString("C", CultureInfo.CurrentCulture)}",
				$"Stock Amount: {StockAmount.ToString("N0")}",
				$"Stock: {Stock}",
			};
			return string.Join("\n\t", companyData);
		}
	}
}
