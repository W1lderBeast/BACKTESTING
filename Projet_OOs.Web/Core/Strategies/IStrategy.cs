using Projet_OOS.Web.Models; // Pour utiliser FinancialData

namespace Projet_OOS.Web.Core.Strategies
{
    // L'interface de stratégie permet à la logique d'être polymorphe
    public interface IStrategy
    {
        // Renvoie une action de trading pour une journée donnée
        TradeAction GetAction(List<FinancialData> historicalData, int currentIndex);

        // Cette propriété est cruciale pour le moteur
        string Name { get; }
    }

    public enum TradeAction
    {
        Hold, // Ne rien faire
        Buy,
        Sell
    }
}