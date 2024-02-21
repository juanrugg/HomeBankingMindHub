using System.Text.Json.Serialization;

namespace HomeBankingMindHub.Models.Dto
{
    public class ClientDto
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<AccountDto> Accounts { get; set; }
        public ICollection<ClientLoanDto> Credits { get; set; }
        public ICollection<CardDto> Cards { get; set; }
    }
}
