﻿namespace HomeBankingMindHub.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client { Email = "vcoronado@gmail.com", FirstName="Victor", LastName="Coronado", Password="123456"},
                    new Client {FirstName = "Salvador", LastName="Ruggeri", Email="test1@123.com", Password="123456"},
                    new Client {FirstName = "Giulia", LastName="Ruggeri", Email="test2@123.com", Password="123456"}
                };

                context.Clients.AddRange(clients);

                //guardamos
                context.SaveChanges();
            }
            if (!context.Accounts.Any())
            {
                var accountSalva = context.Clients.FirstOrDefault(c => c.Email == "test1@123.com");

                if (accountSalva != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountSalva.Id, CreationDate = DateTime.Now, Number = string.Empty, Balance = 0 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();
                }
            }
            if (!context.Transactions.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "VIN001");
                if (account1 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction { AccountId= account1.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia recibida", Type = TransactionType.CREDIT.ToString() },
                        new Transaction { AccountId= account1.Id, Amount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT.ToString() },
                        new Transaction { AccountId= account1.Id, Amount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en CompraGamer", Type = TransactionType.DEBIT.ToString() },
                        new Transaction { AccountId= account1.Id, Amount = 20000, Date= DateTime.Now.AddHours(-8), Description = "Premio Telekino", Type = TransactionType.CREDIT.ToString() }

                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
            }
            if (!context.Loans.Any())
            {
                //Crearemos 3 prestamos Hipotecario, Personal y Automotriz

                var loans = new Loan[] {
                    new Loan {Name="Hipotecario", MaxAmount=500000, Payments="12,24,36,48,60"},
                    new Loan {Name="Personal", MaxAmount=100000, Payments="6,12,24"},
                    new Loan {Name="Automotriz", MaxAmount=300000, Payments="6,12,24,36"}
                };
                foreach (Loan loan in loans)
                {
                    context.Loans.Add(loan);
                }
                context.SaveChanges();
            }

            //ahora agregaremos los clientloan (Prestamos del cliente)
            //usaremos al único cliente que tenemos y le agregaremos un préstamo de cada item

            var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");

            if (client1 != null)
            {
                //ahora usaremos los 3 tipos de prestamos
                var loan1 = context.Loans.FirstOrDefault(I => I.Name == "Hipotecario");
                if (loan1 != null)
                {

                    var clientLoan1 = new ClientLoan
                    {
                        Amount = 400000,
                        ClientId = client1.Id,
                        LoanId = loan1.Id,
                        Payments = "60"
                    };
                    context.ClientLoans.Add(clientLoan1);
                }
                var loan2 = context.Loans.FirstOrDefault(I => I.Name == "Personal");
                if (loan2 != null)
                {
                    var clientLoan2 = new ClientLoan
                    {
                        Amount = 50000,
                        ClientId = client1.Id,
                        LoanId = loan2.Id,
                        Payments = "12"
                    };
                    context.ClientLoans.Add(clientLoan2);
                }
                var loan3 = context.Loans.FirstOrDefault(I => I.Name == "Automotriz");
                if (loan3 != null)
                {
                    var clientLoan3 = new ClientLoan
                    {
                        Amount = 100000,
                        ClientId = client1.Id,
                        LoanId = loan3.Id,
                        Payments = "24"
                    };
                    context.ClientLoans.Add(clientLoan3);
                }

                //guardamos todos los cambios
                context.SaveChanges();
            }


        }
    }


}