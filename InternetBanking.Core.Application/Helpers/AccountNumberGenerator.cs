namespace InternetBanking.Core.Application.Helpers
{
    public static class AccountNumberGenerator
    {
        public static string Generate()
        {
            string accountNumber;

            DateTime currentTimestamp = DateTime.Now;
            string uniqueData = currentTimestamp.ToString("yyyyMMddHHmmssfff");

          
            char[] chars = uniqueData.ToCharArray();
            Random random = new Random();
            for (int i = chars.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                char temp = chars[i];
                chars[i] = chars[j];
                chars[j] = temp;
            }

            accountNumber = new string(chars).Substring(0, 9);
          
            return accountNumber;

        }
    }
}
