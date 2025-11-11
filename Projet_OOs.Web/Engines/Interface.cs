using Projet_OOS.Web.Models;

namespace Projet_OOS.Web.Engines
{
    public interface IBacktestingEngine
    {
        // La méthode prend les données historiques et le nom (ou l'objet) de la stratégie
        Task<BacktestResults> RunBacktestAsync(List<FinancialData> historicalData, string strategyName, decimal initialCapital = 10000);
    }
}