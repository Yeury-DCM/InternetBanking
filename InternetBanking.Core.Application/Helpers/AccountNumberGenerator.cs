namespace InternetBanking.Core.Application.Helpers
{
    public static class AccountNumberGenerator
    {
        public static string Generate()
        {
            string accountNumber;
            Guid guid = Guid.NewGuid();
            var guidHashNumber = Math.Abs(guid.GetHashCode());
            string guidHashString = guidHashNumber.ToString();

            if(guidHashString.Length < 9)
            {
                Random random = new Random();
                int missingLength = 9 - guidHashString.Length;
                string randomFill = random.Next((int)Math.Pow(10, missingLength - 1), (int)Math.Pow(10, missingLength)).ToString();
                guidHashString += randomFill;

            }
            accountNumber = guidHashNumber.ToString().Substring(0, 9);
          
            return accountNumber;

        }
    }
}
