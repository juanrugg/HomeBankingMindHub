
using HomeBankingMindHub.Dto;
using HomeBankingMindHub.Models.Model;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using HomeBankingMindHub.Services.Interfaces;


namespace HomeBankingMindHub.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    public class ClientsController : ControllerBase

    {

        private IClientRepository _clientRepository;
        private readonly IPasswordHasher _passwordHasher;

        public ClientsController(IClientRepository clientRepository, IPasswordHasher passwordHasher)
        {
            _clientRepository = clientRepository;   
            _passwordHasher = passwordHasher;
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
                return Created("",newClient);

            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        //[HttpPost("current/accounts")]
        //public ActionResult PostNewAccount()
        //{
        //    try
        //    {


        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}


    }

}