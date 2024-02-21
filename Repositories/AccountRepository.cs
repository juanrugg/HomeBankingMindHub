
using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
            
        }
        public Account FindById(long id)
        {
            return FindByCondition(Account => Account.Id == id)
                 .Include(Account => Account.Transactions ).FirstOrDefault();
        }


        public IEnumerable<Account> GetAccountsByClient(string Email)
        {
            return FindByCondition(x => x.Client.Email == Email)
                .Include(account => account.Transactions)
                .ToList();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll()
               .Include(Account => Account.Transactions).ToList();
        }
        public void Save(Account account)
        {
            Create(account);
            SaveChanges();
        }

    }
}
