using Projet_OOS.Web.Core;
using Projet_OOS.Web.Core.Strategies;

namespace Projet_OOs.Web.Models
{
    public class Trade
    {
        public DateTime Date { get; set; }
        public SignalType Type { get; set; } // Utiliser SignalType au lieu de TradeAction
        public decimal Quantity { get; set; }
        public decimal Price { get; set; } // Prix d'exécution
        public decimal EquitySnapshot { get; set; }
    }
}
