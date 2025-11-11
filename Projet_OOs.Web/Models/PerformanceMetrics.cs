namespace Projet_OOS.Web.Models
{
    public class PerformanceMetrics
    {
        public string StrategyName { get; set; } = "N/A";
        public decimal InitialCapital { get; set; }
        public decimal FinalCapital { get; set; }
        public decimal TotalReturn { get; set; } // En pourcentage (e.g., 0.15 pour 15%)
        public decimal MaxDrawdown { get; set; } // En pourcentage (e.g., 0.10 pour 10%)
        public int TotalTrades { get; set; }
        public int WinningTrades { get; set; }
        public decimal WinRate { get; set; } // En pourcentage (e.g., 0.60 pour 60%)

        // Ajout d'autres métriques comme le Sharpe Ratio plus tard
    }
}