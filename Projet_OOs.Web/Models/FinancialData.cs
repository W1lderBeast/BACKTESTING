using System.Runtime.InteropServices;
namespace Projet_OOS.Web.Models
{
    // Représente une barre de prix boursier (Open, High, Low, Close, Volume)
    public class FinancialData
    {
        // Clé pour le cache local (si on utilise SQLite/EF Core)
        public int Id { get; set; }

        // CORRECTION CS8618 : Initialisation de la chaîne de caractères
        public string Symbol { get; set; } = string.Empty; // Ticker de l'actif (ex: IBM)

        public DateTime Date { get; set; }

        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal AdjustedClose { get; set; } // Crucial pour le backtesting (dividendes/splits)
        public long Volume { get; set; }
    }

}