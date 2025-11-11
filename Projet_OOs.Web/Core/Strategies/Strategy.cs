using Projet_OOS.Web.Core.Signals;
using Projet_OOS.Web.Models;
using System.Collections.Generic;

namespace Projet_OOS.Web.Core.Strategies
{
    // Classe de base dont héritent toutes les stratégies
    public abstract class Strategy
    {
        // CORRECTION: Ajout de la propriété abstraite 'Name'
        public abstract string Name { get; }

        public virtual void Initialize(IReadOnlyList<FinancialData> history)
        {
        }

        public abstract Signal GenerateSignal(int index, FinancialData currentBar, Portfolio portfolio, IReadOnlyList<FinancialData> history);
    }
}
