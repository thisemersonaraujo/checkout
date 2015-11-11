using System.Security.Cryptography;
using System.Text;

namespace Checkout.Infra.Utilitys
{
    public class Cryptography
    {
        public static string Encrypt(string password)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

                byte[] cryptography = md5.Hash;

                StringBuilder _result = new StringBuilder();

                for (int i = 0; i < cryptography.Length; i++)
                {
                    _result.Append(cryptography[i].ToString("x"));
                }

                return _result.ToString();
            }
            catch
            {
                throw;
            }
        }
    }
}
