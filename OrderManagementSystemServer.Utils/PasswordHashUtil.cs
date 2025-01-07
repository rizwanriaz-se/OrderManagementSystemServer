using System.Security.Cryptography;
using System.Text;

namespace OrderManagementSystemServer.Utils
{
    public class PasswordHashUtil
    {
        private static byte[] m_PasswordSource;
        private static byte[] m_PasswordHash;

        public static string HashPassword(string password)
        {
            m_PasswordSource = Encoding.UTF8.GetBytes(password);
            m_PasswordHash = SHA256.HashData(m_PasswordSource);

            return ByteArrayToString(m_PasswordHash);
        }


        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }


    }
}
