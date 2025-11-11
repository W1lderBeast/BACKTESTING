using Projet_OOS.Web.Core.Strategies; // Ajout pour Signal et SignalType
using Projet_OOS.Web.Models; // Pour FinancialData et Trade
using System.Collections.Generic;
using System.Linq;

namespace Projet_OOS.Web.Core
{
    // Gère l'état du compte pendant la simulation
    public class Portfolio
    {
        // CORRECTION: Changé { get; } en { get; private set; } pour permettre l'initialisation par l'Engine
        public decimal InitialCapital { get; private set; }
        public decimal Cash { get; private set; }
        public Dictionary<string, decimal> Holdings { get; } = new Dictionary<string, decimal>(); // Symbole -> Quantité détenue
        public List<Trade> TradeHistory { get; } = new List<Trade>();
        public Dictionary<DateTime, decimal> EquityCurve { get; } = new Dictionary<DateTime, decimal>(); // Valeur du portefeuille au fil du temps

        private FinancialData? _currentMarketData; // CHANGÉ: Rendu nullable

        // CORRECTION 1: Ajout du constructeur par défaut pour résoudre l'erreur CS7036
        public Portfolio()
        {
        }

        public Portfolio(decimal initialCapital)
        {
            InitialCapital = initialCapital;
            Cash = initialCapital;
        }

        // Ajout d'un SET privé pour permettre au moteur d'initialiser InitialCapital/Cash/TotalEquity
        // Note: TotalEquity est une propriété calculée, elle n'a pas besoin de SET
        public decimal TotalEquity
        {
            get
            {
                // Calcule la valeur totale : Cash + Valeur des positions détenues
                decimal holdingsValue = 0;

                // On utilise AdjustedClose car c'est le prix standard pour le backtesting
                if (_currentMarketData != null && Holdings.ContainsKey(_currentMarketData.Symbol))
                {
                    holdingsValue = Holdings[_currentMarketData.Symbol] * _currentMarketData.AdjustedClose;
                }
                return Cash + holdingsValue;
            }
        }

        public void Update(FinancialData currentBar)
        {
            // Met à jour la barre actuelle pour que TotalEquity puisse être calculé correctement
            _currentMarketData = currentBar;

            // Enregistre la valeur du portefeuille à la date actuelle
            EquityCurve[currentBar.Date] = TotalEquity;
        }

        public void ExecuteTrade(Signal signal)
        {
            // On utilise AdjustedClose comme prix d'exécution
            if (_currentMarketData == null || signal.Quantity == 0) return;

            var trade = new Trade
            {
                Date = _currentMarketData.Date,
                Symbol = _currentMarketData.Symbol,
                Price = _currentMarketData.AdjustedClose, // CORRECTION: Utiliser AdjustedClose
                Quantity = signal.Quantity,
                Type = (TradeType)signal.Type
            };

            if (signal.Type == SignalType.Buy)
            {
                decimal cost = trade.Price * trade.Quantity;
                if (Cash >= cost)
                {
                    Cash -= cost;
                    Holdings[trade.Symbol] = Holdings.GetValueOrDefault(trade.Symbol, 0) + trade.Quantity;
                    trade.EquitySnapshot = TotalEquity; // Snapshot après l'opération
                    TradeHistory.Add(trade);
                }
            }
            else if (signal.Type == SignalType.Sell)
            {
                decimal quantityHeld = Holdings.GetValueOrDefault(trade.Symbol, 0);
                decimal quantityToSell = Math.Min(trade.Quantity, quantityHeld);

                if (quantityToSell > 0)
                {
                    decimal proceeds = trade.Price * quantityToSell;
                    Cash += proceeds;

                    trade.Quantity = quantityToSell;

                    Holdings[trade.Symbol] -= quantityToSell;
                    if (Holdings[trade.Symbol] == 0) Holdings.Remove(trade.Symbol);

                    trade.EquitySnapshot = TotalEquity; // Snapshot après l'opération
                    TradeHistory.Add(trade);
                }
            }
        }
    }
}