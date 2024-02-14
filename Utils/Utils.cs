namespace HomeBankingMindHub.Utils
{
    public class Utils
    {
        public static string GenerateAccountNumber()
        {
            var random = new Random();
            string accountNumber;
            var value = random.Next(0, 99999999);
           // Console.WriteLine(value);
            accountNumber = "VIN-" + value;
           // Console.WriteLine(accountNumber);

            return accountNumber;

        }

        public static string GenerateCardNumber()
        {
            var random = new Random();
            string cardNumber;
            cardNumber = random.Next(1000, 9999).ToString() + "-" + random.Next(1000, 9999).ToString() + "-" + random.Next(1000, 9999).ToString() + "-" + random.Next(1000, 9999).ToString();
            // Console.WriteLine(cardNumber);
            return cardNumber;
        }

        public static string GenerateCvv()
        {
            var random = new Random();
            string cvvNumber;
            cvvNumber = random.Next(1, 999).ToString();
            return cvvNumber;
        }



    }
}
