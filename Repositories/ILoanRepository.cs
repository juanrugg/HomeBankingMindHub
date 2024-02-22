using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Dto;

namespace HomeBankingMindHub.Repositories
{
    public interface ILoanRepository
    {
        public IEnumerable<Loan> GetAllLoans();
        public Loan FindById(long id);
    }
}
