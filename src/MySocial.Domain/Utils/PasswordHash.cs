using System.Security.Cryptography;
using System.Text;

namespace MySocial.Domain.Utils
{
    public static class PasswordHash
    {
        public static string Hash(string input)
        {
            using var md5Hash = MD5.Create();
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();

            foreach (var item in data)
            {
                sBuilder.Append(item.ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
