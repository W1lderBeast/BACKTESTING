using Projet_OOS.Web.Core.Indicators;
using Projet_OOS.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Projet_OOS.Web.Core.Strategies
{
    public class MovingAverageCrossoverStrategy : Strategy
    {
        public int FastPeriod { get; set; } = 50;
        public int SlowPeriod { get; set; } = 200;

        public override string Name => $"MAC ({FastPeriod}/{SlowPeriod})";

        public override Signal GenerateSignal(FinancialData currentBar, Portfolio portfolio, List<FinancialData> history)
        {
            if (history.Count < SlowPeriod + 1)
                return null;

            var fastMaList = IndicatorCalculator.CalculateSMA(history, FastPeriod);
            var slowMaList = IndicatorCalculator.CalculateSMA(history, SlowPeriod);

            int index = history.Count - 1;

            if (index < 1 || fastMaList[index - 1] == null || fastMaList[index] == null ||
                slowMaList[index - 1] == null || slowMaList[index] == null)
                return null;

            var fastMaYesterday = fastMaList[index - 1].Value;
            var fastMaToday = fastMaList[index].Value;
            var slowMaYesterday = slowMaList[index - 1].Value;
            var slowMaToday = slowMaList[index].Value;

            // Croisement haussier
            if (fastMaYesterday <= slowMaYesterday && fastMaToday > slowMaToday && !portfolio.Holdings.Any())
            {
                decimal price = currentBar.AdjustedClose;
                decimal availableCash = portfolio.Cash;
                decimal quantityToBuy = price > 0 ? Math.Floor(availableCash / price) : 0;

                if (quantityToBuy > 0)
                    return new Signal(SignalType.Buy, quantityToBuy);
            }

            // Croisement baissier
            if (fastMaYesterday >= slowMaYesterday && fastMaToday < slowMaToday && portfolio.Holdings.Any())
            {
                var symbol = currentBar.Symbol;
                decimal quantityToSell = portfolio.Holdings.GetValueOrDefault(symbol, 0);

                if (quantityToSell > 0)
                    return new Signal(SignalType.Sell, quantityToSell);
            }

            return null;
        }
    }
}
