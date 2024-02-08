using HomeBankingMindHub.Models;
using System.Text.Json.Serialization;

namespace HomeBankingMindHub.Dto
{
    public class TransactionDto
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string AccountId { get; set; }



    }
}
