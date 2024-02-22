using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Dto;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMindHub.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IClientLoanRepository _clientLoanRepository;
        public LoansController(IAccountRepository accountRepository, IClientRepository clientRepository, ILoanRepository loanRepository,
            ITransactionRepository transactionRepository, IClientLoanRepository clientLoanRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
            _loanRepository = loanRepository;
            _transactionRepository = transactionRepository;
            _clientLoanRepository = clientLoanRepository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var loans = _loanRepository.GetAllLoans();
                var LoansDto = new List<LoanDto>();
                foreach (Loan loan in loans)
                {
                    var newLoanDto = new LoanDto
                    {
                        Id = loan.Id,
                        Name = loan.Name,
                        Payments = loan.Payments,
                        MaxAmount = loan.MaxAmount,
                    };
                    LoansDto.Add(newLoanDto);
                }
                return StatusCode(200, LoansDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoanApplicationDto applicationDto)
        {
            try
            {

                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                //if (email == string.Empty)
                //{
                //    return StatusCode(403, "Email vacio");
                //}
                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return StatusCode(403, "No existe el cliente");
                }

                if (applicationDto.Amount <= 0)
                {
                    return StatusCode(403, "El monto del prestamo no puede ser de CERO pesos");
                }
                Loan loan = _loanRepository.FindById(applicationDto.LoanId);
                if (loan == null)
                {
                    return StatusCode(403, "El tipo de prestamos solicitado no existe");
                }
                if (applicationDto.Amount > loan.MaxAmount)
                {
                    return StatusCode(403, "El monto del prestamo solicitado excede el permitido");
                }
                
                if (applicationDto.Payments.IsNullOrEmpty())
                {
                    return StatusCode(403, "El campo cuotas no puede venir vacio");
                }
                var newPaymentValues = loan.Payments.Split(',').Select(s => s.Trim()).ToList();
                if (!newPaymentValues.Contains(applicationDto.Payments.ToString()))
                {
                    return BadRequest("La cantidad de cuotas ingresadas no es valida para el tipo de prestamo solicitado");
                }
                




                // Buscamos la cuenta de destino

                Account? toAccount = _accountRepository.FindByNumber(applicationDto.ToAccountNumber);
                if (toAccount == null)
                {
                    return StatusCode(403, "La cuenta de destino no existe");
                }
                if(toAccount.ClientId != client.Id)
                {
                    return StatusCode(403, "La cuenta de destino no pertenece al cliente");
                }
                double finalAmount = applicationDto.Amount * 1.2;

                //Generamos un prestamo para el cliente
                _clientLoanRepository.Save(new ClientLoan
                {
                    ClientId = toAccount.ClientId,
                    Amount =  finalAmount,
                    Payments = applicationDto.Payments,
                    LoanId = applicationDto.LoanId,
                });


                //Ahora un credito para la cuenta fromAccount
                _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.CREDIT,
                    Amount = applicationDto.Amount,
                    Description = "Loan approved " + loan.Name,
                    AccountId = toAccount.Id,
                    Date = DateTime.Now,

                });

                //Sumamos el dinero pedido al total de nuestra cuenta

                toAccount.Balance = toAccount.Balance + applicationDto.Amount;

                _accountRepository.Save(toAccount);

                return Created("Prestamo creada con Exito", toAccount);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
