using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace TimeTracker.Login
{
    class LoginLogic
    {
        /// <summary>
        /// Object to interact with the database
        /// </summary>
        private clsDataAccess data = new clsDataAccess();

        public bool CheckUserInfo(string username, string password)
        {
            //TODO: Salt the password, hash it, then check it against the hashed password in the database. If it's good, return true, else return false.
            var salt = data.ExecuteScalarSQL("SELECT Salt FROM [User] WHERE Username =  '" + username + "';");
            var correctHash = data.ExecuteScalarSQL("SELECT Hash FROM [User] WHERE Username =  '" + username + "';");

            var hash = GenerateSHA256Hash(password, salt);

            if (correctHash == hash)
            {
                return true;
            }

            return false;
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
