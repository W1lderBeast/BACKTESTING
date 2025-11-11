using Projet_OOS.Web.Core.Indicators;
using Projet_OOS.Web.Core.Signals;
using Projet_OOS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projet_OOS.Web.Core.Strategies
{
    public class MovingAverageCrossoverStrategy : Strategy
    {
        public int FastPeriod { get; set; } = 50;
        public int SlowPeriod { get; set; } = 200;

        public override string Name => $"MAC ({FastPeriod}/{SlowPeriod})";

        private decimal?[] _fastMa = Array.Empty<decimal?>();
        private decimal?[] _slowMa = Array.Empty<decimal?>();

        public override void Initialize(IReadOnlyList<FinancialData> history)
        {
            _fastMa = IndicatorCalculator.CalculateSMA(history, FastPeriod);
            _slowMa = IndicatorCalculator.CalculateSMA(history, SlowPeriod);
        }

        public override Signal GenerateSignal(int index, FinancialData currentBar, Portfolio portfolio, IReadOnlyList<FinancialData> history)
        {
            if (_fastMa.Length <= index || _slowMa.Length <= index || index < 1)
            {
                return null;
            }

            var fastMaYesterday = _fastMa[index - 1];
            var fastMaToday = _fastMa[index];
            var slowMaYesterday = _slowMa[index - 1];
            var slowMaToday = _slowMa[index];

            if (fastMaYesterday == null || fastMaToday == null || slowMaYesterday == null || slowMaToday == null)
            {
                return null;
            }

            // Croisement haussier
            if (fastMaYesterday.Value <= slowMaYesterday.Value && fastMaToday.Value > slowMaToday.Value && !portfolio.Holdings.Any())
            {
                decimal price = currentBar.AdjustedClose;
                decimal availableCash = portfolio.Cash;
                decimal quantityToBuy = price > 0 ? Math.Floor(availableCash / price) : 0;

                if (quantityToBuy > 0)
                    return new Signal(SignalType.Buy, quantityToBuy);
            }

            // Croisement baissier
            if (fastMaYesterday.Value >= slowMaYesterday.Value && fastMaToday.Value < slowMaToday.Value && portfolio.Holdings.Any())
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
