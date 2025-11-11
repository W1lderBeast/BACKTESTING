using Projet_OOS.Web.Models;
using System;
using System.Collections.Generic;

namespace Projet_OOS.Web.Core.Indicators
{
    public static class IndicatorCalculator
    {
        /// <summary>
        /// Calcule la Simple Moving Average (SMA) alignée avec les données.
        /// Retourne une liste de même longueur que les données, avec null pour les jours non calculables.
        /// </summary>
        public static decimal?[] CalculateSMA(IReadOnlyList<FinancialData> data, int period)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (period <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(period), "Period must be positive.");
            }

            var result = new decimal?[data.Count];

            if (data.Count == 0)
            {
                return result;
            }

            var windowValues = new decimal[period];
            var windowValidity = new bool[period];
            decimal runningSum = 0m;
            int invalidCount = 0;

            for (int i = 0; i < data.Count; i++)
            {
                int bufferIndex = i % period;

                if (i >= period)
                {
                    if (windowValidity[bufferIndex])
                    {
                        runningSum -= windowValues[bufferIndex];
                    }
                    else
                    {
                        invalidCount--;
                    }
                }

                decimal close = data[i].AdjustedClose;
                bool isValid = close > 0;

                windowValues[bufferIndex] = close;
                windowValidity[bufferIndex] = isValid;

                if (isValid)
                {
                    runningSum += close;
                }
                else
                {
                    invalidCount++;
                }

                if (i >= period - 1)
                {
                    result[i] = invalidCount == 0 ? runningSum / period : null;
                }
                else
                {
                    result[i] = null;
                }
            }

            return result;
        }
    }
}
