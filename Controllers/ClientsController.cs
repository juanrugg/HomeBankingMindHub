using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using HomeBankingMindHub.Services.Interfaces;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using HomeBankingMindHub.Utils;
using HomeBankingMindHub.Models.Dto;
using System.Net;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace HomeBankingMindHub.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    public class ClientsController : ControllerBase

    {

        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;
        private ICardRepository _cardRepository;
        public ClientsController(IClientRepository clientRepository, IPasswordHasher passwordHasher,IAccountRepository accountRepository, ICardRepository cardRepository)
        {
            _clientRepository = clientRepository;   
            _passwordHasher = passwordHasher;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
        }

        [HttpGet]

        public IActionResult Get()

        {
            try
            {
                var clients = _clientRepository.GetAllClients();

                var clientsDTO = new List<ClientDto>();

                foreach (Client client in clients)
                {
                    var newClientDTO = new ClientDto
                    {
                        Id = client.Id,
                        Email = client.Email,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Accounts = client.Accounts.Select(ac => new AccountDto
                        {
                            Id = ac.Id,
                            Balance = ac.Balance,
                            CreationDate = ac.CreationDate,
                            Number = ac.Number
                        }).ToList(),
                        Credits = client.ClientLoans.Select(cl => new ClientLoanDto {
                            Id = cl.Id,
                            LoanId = cl.LoanId,
                            Name = cl.Loan.Name,
                            Amount = cl.Amount,
                            Payments = int.Parse(cl.Payments)
                        }).ToList(),
                        Cards = client.Cards.Select(card => new CardDto
                        {
                            Id = card.Id,
                            CardHolder = card.CardHolder,
                            Color = card.Color.ToString(),
                            Type = card.Type.ToString(),
                            Number = card.Number,
                            Cvv = card.Cvv,
                            FromDate = card.FromDate,
                            ThruDate = card.ThruDate
                        }).ToList()
                    };

                    clientsDTO.Add(newClientDTO);
                }
                return Ok(clientsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var client = _clientRepository.FindById(id);
                if (client == null)
                {
                    return Forbid();
                }

                var clientDTO = new ClientDto
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDto
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Credits = client.ClientLoans.Select(cl => new ClientLoanDto
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Cards.Select(card => new CardDto
                    {
                        Id = card.Id,
                        CardHolder = card.CardHolder,
                        Color = card.Color.ToString(),
                        Type = card.Type.ToString(),
                        Number = card.Number,
                        Cvv = card.Cvv,
                        FromDate = card.FromDate,
                        ThruDate = card.ThruDate
                    }).ToList()
                };
                return Ok(clientDTO);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpPost]
        //public IActionResult AddNewClient([FromBody] ClientPost model)
        //{
        //    try
        //    {
        //        if (model.LastName.IsNullOrEmpty() || model.FirstName.IsNullOrEmpty() || model.Email.IsNullOrEmpty())
        //        {
        //            return BadRequest("Alguno de los datos ingresados es incorrecto");
        //        }
        //        var newClient = new Client
        //        {
        //            FirstName = model.FirstName,
        //            LastName = model.LastName,
        //            Email = model.Email,
        //            Password = model.FirstName
        //        };
        //        _clientRepository.Save(newClient);
        //        return Created();
        //    }catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        
        //}

        [HttpGet("current")]
        public IActionResult getCurrent()
        {
            try
            {

                string email = User.FindFirst("Client") != null ?
                    User.FindFirst("Client").Value : string.Empty;
                if(email == string.Empty)
                {
                    return Forbid();
                }
                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid();
                }

                var clientDto = new ClientDto
                {
                    Id = client.Id,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Email = client.Email,
                    Accounts = client.Accounts.Select(ac => new AccountDto
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number,
                    }).ToList(),
                    Credits = client.ClientLoans.Select(lc => new ClientLoanDto
                    {
                        Id = lc.Id,
                        LoanId = lc.LoanId,
                        Name = lc.Loan.Name,
                        Amount = lc.Amount,
                        Payments = int.Parse(lc.Payments),
                    }).ToList(),
                    Cards = client.Cards.Select(card => new CardDto
                    {
                        Id = card.Id,
                        CardHolder = card.CardHolder,
                        Number = card.Number,
                        Color = card.Color.ToString(),
                        Type = card.Type.ToString(),
                        Cvv = card.Cvv,
                        FromDate = card.FromDate,
                        ThruDate = card.ThruDate,
                    }).ToList()
                };

                return Ok(clientDto);

            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] NewClientDto newClientDto)
        {
            try {

                var passwordHash = _passwordHasher.Hash(newClientDto.Password);
                if (string.IsNullOrEmpty(newClientDto.Email) || string.IsNullOrEmpty(newClientDto.Password)
                    || string.IsNullOrEmpty(newClientDto.FirstName) || string.IsNullOrEmpty(newClientDto.LastName))

                    return StatusCode(403, "Datos Invalidos");

                Client user = _clientRepository.FindByEmail(newClientDto.Email);

                if(user != null)
                {
                    return StatusCode(403, "Este correo ya esta registrado");
                }
                 
                Client newClient = new Client()
                {
                    Email = newClientDto.Email,
                    HashedPassword = passwordHash,
                    FirstName = newClientDto.FirstName,
                    LastName = newClientDto.LastName,
                };

                _clientRepository.Save(newClient);
                
                Client client = _clientRepository.FindByEmail(newClient.Email);

                Account newAccount = new Account()
                {
                    Number = Utils.Utils.GenerateAccountNumber(),
                    CreationDate = DateTime.Now,
                    Balance = 0,
                    ClientId = client.Id
                    //Client = client

                };
                _accountRepository.Save(newAccount);

                return StatusCode (201, newClient);

               

            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("current/accounts")]
        public IActionResult getCurrentAccounts()
        {
            try
            {
                string clientEmail = User.FindFirstValue("Client");

                Account[] accounts = _accountRepository.GetAccountsByClient(clientEmail).ToArray();
                AccountDto[] accountDto = new AccountDto[accounts.Count()];
                int index = 0;
                foreach(Account account1 in accounts) 
                {

                    accountDto[index] = new() 
                    {
                        Id = account1.Id,
                        Balance = account1.Balance,
                        CreationDate = account1.CreationDate,
                        Number = account1.Number,
                        Transactions = account1.Transactions.Select(transaction => new TransactionDto
                        {
                            Type = transaction.Type.ToString(),
                            Amount = transaction.Amount,
                            Description = transaction.Description,
                            Date = transaction.Date,
                            Id = transaction.Id
                        }).ToList()
                    };

                    index++;
                }   


                return Ok(accountDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("current/accounts")]
        public ActionResult PostNewAccount()
        {
            try
            {
                string? clientEmail = User.FindFirstValue("Client");
                if (clientEmail.IsNullOrEmpty())
                {
                    return Unauthorized();
                }

                Client client = _clientRepository.FindByEmail(clientEmail);

                Account newAccount = new Account()
                {
                    Number = Utils.Utils.GenerateAccountNumber(),
                    CreationDate = DateTime.Now,
                    Balance = 0,
                    ClientId = client.Id
                    //Client = client

                };
                _accountRepository.Save(newAccount);
                return StatusCode(201);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current/cards")]
        public IActionResult getCurrentCards()
        {
            try { 
            string clientEmail = User.FindFirstValue("Client");

            var cards = _cardRepository.GetAllCardsFrom(clientEmail);
                if (clientEmail.IsNullOrEmpty())
                {
                    return Forbid();
                }
                var cardsDto = new List<CardDto>();
            foreach (Card card in cards)
            {
                var newCardDto = new CardDto
                {
                    Id = card.Id,
                    CardHolder = card.CardHolder,
                    Number = card.Number,
                    Color = card.Color.ToString(),
                    Type = card.Type.ToString(),
                    Cvv = card.Cvv,
                    FromDate = card.FromDate,
                    ThruDate = card.ThruDate,

                };
                cardsDto.Add(newCardDto);
            }

                return Ok(cardsDto);
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("current/cards")]
        public ActionResult PostNewCard([FromBody] NewCardDto model)
        {

            try
            {
                string? clientEmail = User.FindFirstValue("Client");

                if (clientEmail.IsNullOrEmpty())
                {
                    return Unauthorized();
                }

                Client client = _clientRepository.FindByEmail(clientEmail);
                Random random = new Random();

             
                CardType Type = (CardType)Enum.Parse(typeof(CardType), model.Type, true);
                CardColor Color = (CardColor)Enum.Parse(typeof(CardColor), model.Color, true);


                Card newCard = new Card()
                {
                    CardHolder = client.FirstName,
                    Number = Utils.Utils.GenerateCardNumber(),
                    FromDate = DateTime.Now.AddMonths(0),
                    ThruDate = DateTime.Now.AddYears(4),                    
                    Type = (CardType)Enum.Parse(typeof(CardType), model.Type, true),
                    Color = (CardColor)Enum.Parse(typeof(CardColor), model.Color, true),
                    Cvv = random.Next(100,1000),
                    ClientId = client.Id
                };

                if (_cardRepository.ValidateCard(client.Id, Type, Color) == true)
                {
                    return StatusCode(403,"El cliente ya cuenta con una tarjeta de este tipo y color");
                }

                _cardRepository.save(newCard);

                return StatusCode(201);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

    }

}