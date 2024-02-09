using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Repositories
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }

        public Client FindByEmail(string email)
        {
            return FindByCondition(client => client.Email.ToLower() == email.ToLower())
                .Include(client => client.Accounts)
                .Include(client => client.ClientLoans)
                .ThenInclude(cl => cl.Loan)
                .Include(client => client.Cards)
                .FirstOrDefault();
        }

        public Client FindById(long id)
        {
            return FindByCondition(Client => Client.Id == id)
                .Include(Client => Client.Accounts)
                .Include(Client => Client.ClientLoans)
                .ThenInclude(cl => cl.Loan)
                .Include(client => client.Cards)
                .FirstOrDefault();
        }

        public IEnumerable<Client> GetAllClients()
        {
            return FindAll()
                .Include(Client => Client.Accounts)
                .Include(Client => Client.ClientLoans)
                .ThenInclude(cl => cl.Loan)
                .Include(client => client.Cards)
                .ToList();
        }

        public void Save(Client client)
        {
            Create(client);
            SaveChanges();
        }
    }
}
