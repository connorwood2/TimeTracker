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
                sql =
                    "SELECT u.[username], ui.[StartTime], ui.[EndTime], ui.[Comment], ui.[TotalSecondsWorked] FROM [User] AS u INNER JOIN [UserInput] AS ui ON u.[UserID] = ui.[UserID] ORDER BY u.[username];";
            }
            else
            {
                sql =
                    "SELECT u.[username], ui.[StartTime], ui.[EndTime], ui.[Comment], ui.[TotalSecondsWorked] FROM [User] AS u INNER JOIN [UserInput] AS ui ON u.[UserID] = ui.[UserID] WHERE u.[Group] = '" +
                    converted + "' ORDER BY u.[username];";
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

        public DataSet GetTotalHours(string groupNum)
        {
            var converted = Int32.Parse(groupNum);
            string sql = "";
            if (converted == 0)
            {
                sql =
                    "SELECT u.username, SUM(ui.TotalSecondsWorked) FROM [User] u INNER JOIN [UserInput] ui ON u.[UserID] = ui.[UserID] GROUP BY u.[username] ORDER BY u.[username];";
            }

            else
            {
                sql =
                    "SELECT u.username, SUM(ui.TotalSecondsWorked) FROM [User] u INNER JOIN [UserInput] ui ON u.[UserID] = ui.[UserID] WHERE u.[Group] = '" +
                    converted + "' GROUP BY u.[username] ORDER BY u.[username];";
            }


            int num = 100;
            var dt = data.ExecuteSQLStatement(sql, ref num);

            return dt;
        }

        public bool IsObserver(string groupNum)
        {
            var converted = Int32.Parse(groupNum);
            if (converted == 0)
            {
                return true;
            }

            return false;
        }
    }
}