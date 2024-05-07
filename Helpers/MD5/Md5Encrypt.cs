namespace MyBudgetManagement.AppService.MD5Service;

public class Md5Encrypt
{
    public static string Encrypt(string password)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes); 
        }
    }
}