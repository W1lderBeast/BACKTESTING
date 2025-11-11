using Projet_OOS.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Projet_OOS.Web.Core.Indicators
{
    public static class IndicatorCalculator
    {
        /// <summary>
        /// Calcule la Simple Moving Average (SMA) alignée avec les données.
        /// Retourne une liste de même longueur que les données, avec null pour les jours non calculables.
        /// </summary>
        public static List<decimal?> CalculateSMA(List<FinancialData> data, int period)
        {
            var smaList = new List<decimal?>();

            for (int i = 0; i < data.Count; i++)
            {
                if (i < period - 1)
                {
                    smaList.Add(null); // Pas assez de données pour le calcul
                    continue;
                }

                // Prendre les 'period' dernières valeurs (AdjustedClose)
                var closingPrices = data.Skip(i - period + 1)
                                        .Take(period)
                                        .Select(d => d.AdjustedClose)
                                        // ✅ SÉCURISATION : Filtrer les prix à zéro qui fausseraient la moyenne
                                        .Where(p => p > 0)
                                        .ToList();

                // Si la liste filtrée n'a pas la taille requise (c'est-à-dire qu'il manque des données > 0), 
                // on ne peut pas calculer une moyenne correcte.
                if (closingPrices.Count < period)
                {
                    smaList.Add(null);
                    continue;
                }

                // Utilise Average() sur Enumerable<decimal> qui retourne decimal?
                smaList.Add(closingPrices.Average());
            }

            return smaList;
        }
    }
}