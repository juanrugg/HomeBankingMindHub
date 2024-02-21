using HomeBankingMindHub.Models;


namespace HomeBankingMindHub.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
            
        }
        public Transaction FindByNumber(long Id)
        {
            return FindByCondition(transaction => transaction.Id == Id).FirstOrDefault();
        }

        public void Save(Transaction transaction)
        {
            Create(transaction);
            SaveChanges();
        }

      
    }
}
