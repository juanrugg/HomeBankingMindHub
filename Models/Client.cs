using Microsoft.EntityFrameworkCore;
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
        public string Password { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
