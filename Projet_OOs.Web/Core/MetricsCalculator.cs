using Projet_OOS.Web.Models;
using System.Linq;

namespace Projet_OOS.Web.Core
{
    public class MetricsCalculator
    {
        public PerformanceMetrics Calculate(Portfolio portfolio, string strategyName)
        {
            var metrics = new PerformanceMetrics
            {
                StrategyName = strategyName,
                InitialCapital = portfolio.InitialCapital,
                FinalCapital = portfolio.TotalEquity, // La valeur finale est la dernière TotalEquity
                TotalTrades = portfolio.TradeHistory.Count
            };

            // 1. Calcul du Rendement Total (Total Return)
            metrics.TotalReturn = (metrics.FinalCapital - metrics.InitialCapital) / metrics.InitialCapital;

            // 2. Calcul du Max Drawdown
            // Le Drawdown est calculé sur la courbe de capital (EquityCurve)
            metrics.MaxDrawdown = CalculateMaxDrawdown(portfolio.EquityCurve.Values.ToList());

            // 3. Calcul du Taux de Gain (Win Rate)
            var trades = portfolio.TradeHistory;

            if (trades.Count > 0)
            {
                // Nous devons parcourir l'historique pour déterminer si chaque trade Buy/Sell est profitable.
                // NOTE: La logique de profit/perte est plus complexe car on ne sait pas quelle vente correspond à quel achat.
                // Pour une implémentation simple, nous allons simplifier et compter le nombre total de trades pour l'instant.

                // Pour une mesure plus précise, nous devons traiter les trades comme des paires (entrée/sortie).
                // Cependant, pour l'instant, nous allons nous concentrer sur le nombre de transactions totales.

                // Si votre moteur n'implémente que des positions 0/100% (tout ou rien), 
                // on peut simplifier en comptant les transactions complètes (Buy suivi de Sell).

                // Ici, nous allons compter les trades où le capital après le trade a augmenté par rapport au capital initial
                // (cette métrique est imprécise mais simple).

                // Laissez la WinRate à 0 pour l'instant, car la détermination des paires de trades Buy/Sell 
                // est un niveau de complexité que nous pouvons aborder séparément.

                // Si vous avez un champ de profit/perte par trade dans votre modèle Trade, utilisez-le ici.

                // Pour l'instant, nous comptons simplement les trades Buy/Sell :
                metrics.WinningTrades = 0; // à implémenter correctement
                metrics.WinRate = 0;
            }

            return metrics;
        }

        /// <summary>
        /// Calcule le Maximum Drawdown (en pourcentage) à partir de la courbe de capital.
        /// </summary>
        /// <param name="equityCurve">Liste des valeurs du portefeuille au fil du temps.</param>
        /// <returns>Le Max Drawdown sous forme décimale (e.g., 0.10).</returns>
        private decimal CalculateMaxDrawdown(List<decimal> equityCurve)
        {
            if (!equityCurve.Any())
                return 0;

            decimal maxDrawdown = 0;
            decimal peak = equityCurve.First();

            foreach (var equity in equityCurve)
            {
                // Mettre à jour le pic (Peak)
                if (equity > peak)
                {
                    peak = equity;
                }

                // Calculer le drawdown actuel
                decimal drawdown = (peak - equity) / peak;

                // Mettre à jour le Max Drawdown
                if (drawdown > maxDrawdown)
                {
                    maxDrawdown = drawdown;
                }
            }

            return maxDrawdown;
        }
    }
}