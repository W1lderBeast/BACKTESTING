using Projet_OOS.Web.Core.Strategies; // Pour la référence à Trade

namespace Projet_OOS.Web.Models
{
    public class Portfolio
    {
        public decimal InitialCapital { get; set; }
        public decimal Cash { get; set; } // Argent liquide restant
        public decimal TotalEquity { get; set; } // Capital total (Cash + valeur des actifs)

        // Correspond à la structure utilisée dans votre stratégie
        // Holdings stocke le symbole et la quantité détenue
        public Dictionary<string, decimal> Holdings { get; set; } = new Dictionary<string, decimal>();

        public List<Trade> TradeHistory { get; set; } = new List<Trade>();

        public Portfolio()
        {
            // Constructeur par défaut
        }
    }
}