using System.Security.Cryptography;
using System.Text;

namespace UserControl.Services
{
    public static class EncryptPassword
    {
         // c# string to sha256
        // https://www.codegrepper.com/code-examples/csharp/c%23+string+to+sha256
        public static string EcryptUserPassword(string password)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(password));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}