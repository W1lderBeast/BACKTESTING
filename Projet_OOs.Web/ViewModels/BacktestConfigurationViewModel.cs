using System.Runtime.InteropServices;
using Projet_OOS.Web.Core.Strategies;
using System.ComponentModel.DataAnnotations;

namespace Projet_OOS.Web.ViewModels
{
    public class BacktestConfigurationViewModel
    {
        [Required(ErrorMessage = "Le symbole de l'actif est requis.")]
        [StringLength(5, MinimumLength = 1, ErrorMessage = "Le symbole doit comporter entre 1 et 5 caractères.")]
        public string Symbol { get; set; } = "IBM"; // Valeur par défaut pour l'exemple

        [Required(ErrorMessage = "Le capital initial est requis.")]
        [Range(100, 1000000, ErrorMessage = "Le capital doit être compris entre 100 et 1 000 000.")]
        public decimal InitialCapital { get; set; } = 10000;

        // Paramètres spécifiques à la stratégie Moving Average Crossover (MAC)
        public int MacFastPeriod { get; set; } = 50;
        public int MacSlowPeriod { get; set; } = 200;

        // Permet de stocker la stratégie sélectionnée par l'utilisateur
        public string SelectedStrategyType { get; set; } = "MAC";
    }
}