using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.SignUp
{
    class SignUpLogic
    {
        private clsDataAccess data = new clsDataAccess();
        public bool RegisterUser(string username, string password, string group)
        {
            //string sql = "SELECT Username FROM [User] WHERE Username = 'Bilbo'";
            var user = data.ExecuteScalarSQL("SELECT Username FROM [User] WHERE Username = '" + username + "';");

            if (username == user)
            {
                return false;
            }

            string type = "";

            if (group == "0")
            {
                type = "Observer";
            }
            else
            {
                type = "User";
            }

            var salt = CreateSalt();

            var hash = GenerateSHA256Hash(password, salt);

            data.ExecuteNonQuery("INSERT INTO [User] ([Username], [Hash], [Salt], [Type], [Group]) VALUES ('" + username + "', '" + hash + "', '" + salt + "', '" + type + "', '" + group + "');");

            return true;
        }

        public string CreateSalt()
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[8];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        public string GenerateSHA256Hash(String input, String salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            System.Security.Cryptography.SHA256Managed sha256HashString = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256HashString.ComputeHash(bytes);

            var converted = Encoding.UTF8.GetString(hash, 0, hash.Length);

            return converted;
        }
    }
}
