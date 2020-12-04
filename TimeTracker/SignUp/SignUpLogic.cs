using System;
using System.Collections.Generic;
using System.Data;
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

            //If the username already exists, return true. Else add a new user and return false.
            if (username == user)
            {
                return true;
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

            //Get rid of all single and double quotes
            while(hash.Contains("'") || hash.Contains("\"") || salt.Contains("'") || salt.Contains("\""))
            {
                salt = CreateSalt();
                hash = GenerateSHA256Hash(password, salt);
            }
            //If it fials, get a new salt and hash and try again.
            while (data.ExecuteNonQuery("INSERT INTO [User] ([Username], [Hash], [Salt], [Type], [Group]) VALUES ('" + username + "', '" + hash + "', '" + salt + "', '" + type + "', '" + group + "');") == -1){
                salt = CreateSalt();
                hash = GenerateSHA256Hash(password, salt);;
            };

            return false;
        }

        public string CreateSalt()
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[8];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        public DataSet GetAllGroupNumbers()
        {
            int numRows = 0;
            string sql = "SELECT DISTINCT [Group] FROM [User] ORDER BY [Group];";
            return data.ExecuteSQLStatement(sql, ref numRows);
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
