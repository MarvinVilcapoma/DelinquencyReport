namespace ArtSolutions.MUN.Core.Collections
{
    public class PaymentOptionModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int PaymentOptionTableValue { get; set; }
        public string Notes { get; set; }
    }
}
