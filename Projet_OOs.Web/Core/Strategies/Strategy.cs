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

        public abstract Signal GenerateSignal(FinancialData currentBar, Portfolio portfolio, List<FinancialData> history);
    }
}
