namespace StockExchangeAnalysisApp.Models
{
    public class StockPrice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } // Corresponds to "Date"
        public decimal LastTradePrice { get; set; } // Corresponds to "Last trade price"
        public decimal MaxPrice { get; set; } // Corresponds to "Max"
        public decimal MinPrice { get; set; } // Corresponds to "Min"
        public decimal AvgPrice { get; set; } // Corresponds to "Avg. Price"
        public decimal PercentageChange { get; set; } // Corresponds to "%chg."
        public int Volume { get; set; } // Corresponds to "Volume"
        public decimal TurnoverBEST { get; set; } // Corresponds to "Turnover in BEST in denars"
        public decimal TotalTurnover { get; set; } // Corresponds to "Total turnover in denars"
        public int IssuerId { get; set; } // Foreign key reference to Issuer
        public Issuer Issuer { get; set; } // Navigation property to Issuer
    }
}
