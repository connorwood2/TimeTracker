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
            int numRows = 0;
            DataSet ds = data.ExecuteSQLStatement("SELECT Username, Hash FROM User WHERE Username = \"" + username + "\"", ref numRows);

            //TODO: Salt the password, hash it, then check it against the hashed password in the database. If it's good, return true, else return false.
            return false;
        }

    }
}
