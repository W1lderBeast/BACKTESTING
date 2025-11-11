namespace Projet_OOS.Web.Core.Signals
{
    public enum SignalType
    {
        Hold, // Ne rien faire
        Buy,  // Acheter
        Sell  // Vendre
    }

    public class Signal
    {
        public SignalType Type { get; set; }
        public decimal Quantity { get; set; }

        public Signal(SignalType type, decimal quantity = 0)
        {
            Type = type;
            Quantity = quantity;
        }
    }
}
