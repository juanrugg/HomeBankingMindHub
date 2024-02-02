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

        }
    }
}

