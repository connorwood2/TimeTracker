using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TimeTracker.Input
{
    /// <summary>
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class Input : Window
    {
        private InputLogic inputLogic = new InputLogic();

        private string _username;

        private int _seconds = 0;

        private DateTime StartTime = DateTime.MinValue;

        private DateTime EndTime = new DateTime();


        // Default constructor
        //public Input()
        //{
        //    InitializeComponent();
        //}

        public Input(string username)
        {
            InitializeComponent();
            
            _username = username;
            var groupNum = inputLogic.GetGroupNumber(_username);
            if (inputLogic.IsObserver(groupNum))
            {
                btnStartTime.IsEnabled = false;
                btnStopTime.IsEnabled = false;
                txtboxComment.IsEnabled = false;
                btnResetTime.IsEnabled = false;
                btnInsertData.IsEnabled = false;
            }

            //Populate the combo box
            DataSet groupNums = inputLogic.GetAllGroupNumbers();
            for (int i = 0; i < groupNums.Tables[0].Rows.Count; i++)
            {
                //Observers should not be able to be chosen
                if(groupNums.Tables[0].Rows[i][0].ToString() == "0")
                {
                    continue;
                }
                cbxGroupSelection.Items.Add(groupNums.Tables[0].Rows[i][0].ToString());
            }


        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if(cbxGroupSelection.SelectedItem == null)
            {
                lblError.Content = "You must first select a group";
                lblError.Visibility = Visibility.Visible;
                return;
            }

            lblError.Visibility = Visibility.Hidden;
            var groupNum = cbxGroupSelection.SelectedItem.ToString();
            PopulateResults(groupNum);
        }

        private void cbxGroupSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            lblError.Visibility = Visibility.Hidden;
            var groupNum = cbxGroupSelection.SelectedItem.ToString();
            PopulateResults(groupNum);
        }

        private void PopulateResults(string groupNum)
        {
            var view = inputLogic.ViewAll(groupNum);

            var pieChartView = inputLogic.GetTotalHours(groupNum);
            var table = pieChartView.Tables[0];

            dataGrid.ItemsSource = new DataView(view.Tables[0]); // Grab [User] Table

            ((PieSeries)PieChart.Series[0]).ItemsSource = table.AsEnumerable().Select(grp =>
               new KeyValuePair<string, string>(grp[0].ToString(), grp[1].ToString()));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login.Login login = new Login.Login();
            login.Show();
            Close();
            return;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (StartTime != DateTime.MinValue)
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Content = "Error: Start time has already been set -  reset time to start again.";
                return;
            }

            lblError.Visibility = Visibility.Hidden;
            StartTime = DateTime.Now;
            lblStart.Visibility = Visibility.Visible;
            lblStart.Content = $"Start Time: {StartTime}";
            lblError.Visibility = Visibility.Hidden;
        }

        private void btnStopTime_Click(object sender, RoutedEventArgs e)
        {
            if (StartTime == DateTime.MinValue)
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Content = "Error. You have not started the time.";
                return;
            }

            lblError.Visibility = Visibility.Hidden;
            EndTime = DateTime.Now;
            lblEnd.Visibility = Visibility.Visible;
            lblEnd.Content = $"Stop Time: {EndTime}";

            //var minutes = (EndTime.Subtract(StartTime).TotalMinutes);

            //lblTotalTime.Content = $"Total minutes tracked: {minutes}";

            var difference = EndTime - StartTime;
            lblTotalTime.Content = ($"Total Time: {difference.Hours:00}:{difference.Minutes:00}:{difference.Seconds + difference.Milliseconds / 1000.0:00}");

            _seconds = difference.Seconds + difference.Minutes * 60 + difference.Hours * 3600;

            lblTotalTime.Visibility = Visibility.Visible;
        }

        private void btnResetTime_Click(object sender, RoutedEventArgs e)
        {
            lblError.Visibility = Visibility.Hidden;
            lblStart.Content = "Start Time:";
            lblEnd.Content = "Stop Time:";
            lblTotalTime.Content = "Total Time:";
            txtboxComment.Text = "";
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MinValue;
            _seconds = 0;
        }

        private void btnInsertData_Click(object sender, RoutedEventArgs e)
        {
            if(StartTime == DateTime.MinValue)
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Content = "Error: Missing a required field - Start Time";
                return;
            }
            if(EndTime == DateTime.MinValue)
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Content = "Error: Missing a required field - Stop Time";
                return;
            }
            if(txtboxComment.Text == "")
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Content = "Error: Missing a required field - comment";
                return;
            }
            if(txtboxComment.Text.Length < 5)
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Content = "Error: Comment must be 5 characters long";
                return;
            }
            lblError.Visibility = Visibility.Hidden;
            inputLogic.InsertData(_username, StartTime, EndTime, txtboxComment.Text, _seconds);
            lblStart.Content = "Start Time:";
            lblEnd.Content = "Stop Time:";
            lblTotalTime.Content = "Total Time:";
            txtboxComment.Text = "";
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MinValue;
            _seconds = 0;
        }
    }
}
