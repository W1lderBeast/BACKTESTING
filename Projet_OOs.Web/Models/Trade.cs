using System;

namespace Projet_OOS.Web.Models
{
    // Modèle simple pour une transaction enregistrée
    public class Trade
    {
        public DateTime Date { get; set; }

        // CORRECTION CS8618 : Initialisation de la chaîne de caractères
        public string Symbol { get; set; } = string.Empty;

        public TradeType Type { get; set; }
        public decimal Price { get; set; } // Prix d'exécution
        public decimal Quantity { get; set; }
        public decimal EquitySnapshot { get; internal set; }
    }

    public enum TradeType
    {
        Buy,
        Sell
    }
}
