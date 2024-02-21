using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Dto;

namespace HomeBankingMindHub.Repositories
{
    public interface ICardRepository
    {
        void save (Card card);

        public bool ValidateCard(long ClientId, CardType Type, CardColor Color);

        IEnumerable<Card> GetAllCardsFrom(string Email);

    }
}
