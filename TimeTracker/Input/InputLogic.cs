using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;

namespace TimeTracker.Input
{
    class InputLogic
    {
        private clsDataAccess data = new clsDataAccess();

        public DataSet ViewAll(string groupNum)
        {
            var converted = Int32.Parse(groupNum);

            string sql = "";

            if (converted == 0)
            {
                sql = "SELECT * FROM [User]";
            }
            else
            {
                sql = "SELECT * FROM [User] WHERE [Group] = '" + converted + "';";
            }

            int num = 100;
            var view = data.ExecuteSQLStatement(sql, ref num);

            //DataTable dt = view.Tables[0];

            return view;
        }

        public string GetGroupNumber(string username)
        {
            var sql = "SELECT Group FROM [User] WHERE Username =  '" + username + "';";
            var groupNum = data.ExecuteScalarSQL(sql);

            return groupNum;
        }
    }
}
