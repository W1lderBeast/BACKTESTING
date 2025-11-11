using System.Runtime.InteropServices;
using Projet_OOS.Web.Models;

namespace Projet_OOS.Web.Core
{
    public enum SignalType { Buy, Sell, Hold,
        None
    }

    // Représente le résultat de l'évaluation de la stratégie
    public class Signal
    {
        public SignalType Type { get; set; }
        public decimal SuggestedQuantity { get; set; } = 0; // Quantité à acheter/vendre
        public int Quantity { get; internal set; }

        public Signal(SignalType type, decimal quantity = 0)
        {
            Type = type;
            SuggestedQuantity = quantity;
        }
    }

    // Classe de base (abstraite) que toutes les stratégies doivent hériter
    public abstract class Strategy
    {
        // Chaque stratégie doit implémenter cette méthode pour générer une recommandation
        public abstract Signal GenerateSignal(FinancialData currentBar, Portfolio portfolio, List<FinancialData> history);
    }
}