namespace HomeBankingMindHub.Models.Dto
{
    public class AccountDto
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
        public ICollection<TransactionDto> Transactions { get; set; }
    }
}
