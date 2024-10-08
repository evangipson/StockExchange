﻿using System.Globalization;
using StockExchange.Base.Serialization.Models;

namespace StockExchange.Domain.Models
{
	/// <summary>
	/// Represents any company that has <see cref="Models.Stock"/>.
	/// </summary>
	public class Company : ISerializedEntity
	{
		public string Name { get; set; } = string.Empty;

		public string TickerName { get; set; } = string.Empty;

		public string? Description { get; set; }

		public string? CEO { get; set; }

		public int Employees { get; set; }

		public string? Headquarters { get; set; }

		public DateTime Founded { get; set; }

		public int StockAmount { get; set; }

		public DateTime IPODate { get; set; }

		public decimal CostOfGoodsSold { get; set; }

		public decimal NetSales { get; set; }

		public decimal Debt { get; set; }

		public decimal NonOperatingItems { get; set; }

		public List<decimal> Expenses { get; set; } = [];

		public List<StockPrice> StockPrices { get; set; } = [];

		public List<StockPrice> StockPricesByDate => [.. StockPrices.OrderBy(stockPrice => stockPrice.Date)];

		public decimal? LatestStockPrice => StockPricesByDate.FirstOrDefault()?.Amount ?? null;

		public int DaysOfData
		{
			get
			{
				if(StockPricesByDate == null || StockPricesByDate.Count < 2)
				{
					return 0;
				}
				var days = new TimeSpan(StockPricesByDate.Last().Date.Ticks - StockPricesByDate.First().Date.Ticks).Days;
                return days == 0 ? 1 : days;
            }
        }

		public decimal GrossProfit => NetSales - CostOfGoodsSold;

		public decimal OperatingExpenses => Expenses.Sum();

		public decimal OperatingIncome => GrossProfit - OperatingExpenses;

		public decimal NetIncome => OperatingIncome - NonOperatingItems;

		// ISerializedEntity properties
		public string ElementName => "Company";

		public string FileName => "companies.save";

		public Guid EntityId { get; set; }

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
				$"Stock Amount: {StockAmount.ToString("N0")}"
			};
			return string.Join("\n\t", companyData);
		}
	}
}
