﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace HomeBankingMindHub.Models
{
    public class Client
    {
        [Key]
        public long Id { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public ICollection<ClientLoan> ClientLoans { get; set; }
        public ICollection<Card> Cards { get; set; }

    }
}
