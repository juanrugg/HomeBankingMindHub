using System.Text.Json.Serialization;

namespace HomeBankingMindHub.Dto
{
    public class ClientDto
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<AccountDto> Accounts { get; set; }
        public ICollection<ClientLoanDto> Loans { get; set; }
    }
}
