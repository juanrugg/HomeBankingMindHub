using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Repositories
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAllClients();
        void Save(Client client);
        Client FindById(long id);
        Client FindByEmail(string email);
        //void Save(Account account);
        //IEnumerable<Account> GetAccountsByClient(long clientId);
    }
}
