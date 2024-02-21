using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Drawing;

namespace HomeBankingMindHub.Repositories
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
       
    {
       
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }

        public IEnumerable<Card> GetAllCardsFrom(string Email)
        {
            return FindByCondition(x => x.Client.Email == Email)
               .Include(Account => Account.Client.Cards).ToList();
        }

        public void save(Card card)
        {
            Create(card);
            SaveChanges();            
        }

        public bool ValidateCard(long ClientId, CardType Type, CardColor Color) => FindByCondition(x => x.ClientId == ClientId)
                                                                     .Where(x => x.Type == (CardType)Type)
                                                                     .Where(x => x.Color == (CardColor)Color).Any();


    }
    }

