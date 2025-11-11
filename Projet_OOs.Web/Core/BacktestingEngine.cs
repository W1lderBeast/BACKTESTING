using Projet_OOS.Web.Core.Signals;
using Projet_OOS.Web.Core.Strategies;
using Projet_OOS.Web.Models;
using Projet_OOS.Web.Services;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projet_OOS.Web.Core
{
    public class BacktestingEngine
    {
        public async Task<Portfolio> RunBacktestAsync(
            List<FinancialData> historicalData,
            Strategies.Strategy strategy,
            decimal initialCapital)
        {
            var portfolio = new Portfolio(initialCapital);

            var symbol = historicalData.FirstOrDefault()?.Symbol;

            strategy.Initialize(historicalData);

            int startIndex = 0;

            if (historicalData.Count > 0)
            {
                // On prend la période lente pour avoir assez de données pour le premier calcul
                if (strategy is MovingAverageCrossoverStrategy macStrategy)
                {
                    startIndex = (int)macStrategy.SlowPeriod;
                }
                // S'assurer qu'on ne dépasse pas la taille des données, et qu'on commence au moins à l'index 1.
                if (startIndex >= historicalData.Count)
                {
                    startIndex = historicalData.Count > 1 ? historicalData.Count - 1 : 0;
                }
            }

            for (int i = startIndex; i < historicalData.Count; i++)
            {
                var currentBar = historicalData[i];

                // 1. Mise à jour de la barre actuelle
                portfolio.Update(currentBar);

                // 2. Obtenir le signal de la stratégie
                var signal = strategy.GenerateSignal(i, currentBar, portfolio, historicalData);

                // 3. Exécuter le trade
                // ✅ Correction : N'exécuter le trade QUE si un signal non nul a été généré.
                if (signal != null)
                {
                    portfolio.ExecuteTrade(signal);
                }
            }

            // 4. Clôturer toute position restante
            if (symbol != null && portfolio.Holdings.GetValueOrDefault(symbol, 0) > 0)
            {
                var lastDay = historicalData.Last();
                decimal positionToClose = portfolio.Holdings[symbol];

                var finalSellSignal = new Signal(SignalType.Sell, positionToClose);

                portfolio.Update(lastDay);

                portfolio.ExecuteTrade(finalSellSignal);
            }

            return portfolio;
        }
    }
}