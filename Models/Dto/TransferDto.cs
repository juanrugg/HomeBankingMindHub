namespace HomeBankingMindHub.Models.Dto
{
    public class TransferDto
    {
        public string FromAccountNumber { get; set; }
        public string ToAccountNumber { get; set;}
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
