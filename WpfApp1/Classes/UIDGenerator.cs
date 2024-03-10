using System.Management;
using System.Security.Cryptography;
using System.Text;

public static class UIDGenerator
{
    public static string GenerateUID()
    {
        string computerIdentifier = GetComputerIdentifier();
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(computerIdentifier);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    private static string GetComputerIdentifier()
    {
        string identifier = "";
        using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct"))
        {
            foreach (ManagementObject obj in searcher.Get())
            {
                identifier = obj["UUID"].ToString();
                break;
            }
        }
        return identifier;
    }
}