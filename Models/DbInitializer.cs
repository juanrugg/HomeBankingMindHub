namespace HomeBankingMindHub.Models
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

                        new Transaction { AccountId= account1.Id, Amount = 20000, Date= DateTime.Now.AddHours(-8), Description = "Premio Telekino", Type = TransactionType.DEBIT.ToString() }

                    };

                    foreach (Transaction transaction in transactions)

                    {

                        context.Transactions.Add(transaction);

                    }

                    context.SaveChanges();



                }

            }

        }
    }
}

