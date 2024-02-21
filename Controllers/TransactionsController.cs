using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Dto;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(IClientRepository clientRepository, IAccountRepository accountRepository,
            ITransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] TransferDto transferDto)
        {
            try { 
            string email = User.FindFirst("Client") != null? User.FindFirst("Client").Value : string.Empty;
            if(email == string.Empty)
            {
                return StatusCode(403,"Email vacio");
            }
            Client client = _clientRepository.FindByEmail(email);
            if(client == null)
            {
                return StatusCode(403, "No existe el cliente");
            }
            if(transferDto.ToAccountNumber == transferDto.FromAccountNumber)
            {
                return StatusCode(403, "La cuenta de origen y de destino no pueden ser iguales");
            }
            if(transferDto.FromAccountNumber == string.Empty || transferDto.ToAccountNumber == string.Empty) 
            {
                return StatusCode(403, "Cuenta de origen o cuenta de destino no proporcionada");
            }
            if(transferDto.Amount <= 0) {
                return StatusCode(403, "El monto a transferir no puede ser igual o menor a CERO Pesos");
            }
            if (transferDto.Description == string.Empty) 
            {
                return StatusCode(403, "Descripcion no proporcionada");
            }

            // Buscamos las cuentas

            Account? fromAccount = _accountRepository.FindByNumber(transferDto.FromAccountNumber);
            if(fromAccount == null)
            {
                return StatusCode(403, "Cuenta de origen no existe");
            }
            // Controlamos el monto
            if((fromAccount.Balance < transferDto.Amount))
            {
                return StatusCode(403, "Fondos Insuficientes");
            }

            // Buscamos la cuenta de destino
            Account? toAccount = _accountRepository.FindByNumber(transferDto.ToAccountNumber);
            if(toAccount == null)
            {
                return StatusCode(403, "La cuenta de destino no existe");
            }

                // De mas logica para guardado
                // comenzamos con la insercion de las 2 transcaciones realizadas
                // desde toAccount se debe generar un debito por lo tanto lo multimplicamos por -1

                _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.DEBIT,
                    Amount = transferDto.Amount * -1,
                    Description = transferDto.Description + " " + toAccount.Number,
                    AccountId = fromAccount.Id,
                    Date = DateTime.Now,
                });

                //Ahora un credito para la cuenta fromAccount
                _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.CREDIT,
                    Amount = transferDto.Amount,
                    Description = transferDto.Description + " " + fromAccount.Number,
                    AccountId = toAccount.Id,
                    Date = DateTime.Now,

                });


            // Seteamos los valores de las cuentas
            
            fromAccount.Balance = fromAccount.Balance - transferDto.Amount;

            // Actualizamos la cuenta de origen

            _accountRepository.Save(fromAccount);

            // A la cuenta de destino le sumamos el monto recibido

            toAccount.Balance = toAccount.Balance + transferDto.Amount;

            // Actualizamos la cuenta de destino

            _accountRepository.Save(toAccount);

            return Created("Transaccion creada con Exito", fromAccount);
            
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
