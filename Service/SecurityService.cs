using System;
using System.Security.Cryptography;
using System.Text;

namespace MMB.Mangalam.Web.Service
{
    public class SecurityService
    {
        public string HashUserNameAndPassword(string userName, string password)
        {

            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(userName.ToLower() + password));
                // Get the hashed string.  
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                // Print the string.   
                return hash;
            }
        }
    }
}
