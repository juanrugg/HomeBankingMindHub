namespace HomeBankingMindHub.Models.Dto
{
    public class LoanApplicationDto
    {
        public long LoanId { get; set; }
        public double Amount { get; set; }
        public string Payments { get; set; }
        public string ToAccountNumber { get; set; }
    }
}
