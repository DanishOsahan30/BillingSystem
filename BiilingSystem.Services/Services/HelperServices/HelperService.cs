namespace BillingSystem.Services.HelperServices
{
    public class HelperService:IHelperService
    {
        public string CapitalizedString(string UserName)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                return string.Empty;
            }
            UserName = UserName.Trim();
            // Remove any spaces from the username
            string trimmedUserName = UserName.Replace(" ", "");


            // Capitalize the first letter of the username
            return char.ToUpper(trimmedUserName[0]) + trimmedUserName[1..];
        }

        public int GenerateRandomNumber()
        {
            Random random = new Random();
            int num = random.Next(10000, 99999);
            return num;
        }
    }
}
