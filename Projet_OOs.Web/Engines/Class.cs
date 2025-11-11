using Projet_OOS.Web.Models;

namespace Projet_OOS.Web.Models
{
    public class BacktestResults
    {
        public decimal InitialCapital { get; set; }
        public decimal FinalCapital { get; set; }
        public decimal CumulativeReturn { get; set; }
        public decimal MaxDrawdown { get; set; }
        // Liste des trades effectués (crucial pour l'affichage)
        public List<Trade> Trades { get; set; } = new List<Trade>();
        public List<FinancialData> DataPoints { get; set; } = new List<FinancialData>(); // Pour les graphiques
    }
}