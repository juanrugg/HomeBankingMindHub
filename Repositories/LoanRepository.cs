using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Repositories
{
    public class LoanRepository : RepositoryBase<Loan>, ILoanRepository
    {
        public LoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }
        
        public Loan FindById(long id)
        {
            return FindByCondition(Loan => Loan.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Loan> GetAllLoans()
        {
            return FindAll();
        }

    }
}
