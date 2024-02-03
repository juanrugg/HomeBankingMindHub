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

        }
    }
}

