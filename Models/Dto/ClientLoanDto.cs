﻿namespace HomeBankingMindHub.Models.Dto
{
    public class ClientLoanDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long LoanId { get; set; }
        public double Amount { get; set; }
        public int Payments { get; set; }
    }
}
