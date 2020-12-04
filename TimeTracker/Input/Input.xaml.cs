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
            }
            
            
        }

        private void btnViewAll_Click(object sender, RoutedEventArgs e)
        {
            var groupNum = inputLogic.GetGroupNumber(_username);

            var view = inputLogic.ViewAll(groupNum);

            var pieChartView = inputLogic.GetTotalHours(groupNum);
            var table = pieChartView.Tables[0];

            dataGrid.ItemsSource = new DataView(view.Tables[0]); // Grab [User] Table

            ((PieSeries) PieChart.Series[0]).ItemsSource = table.AsEnumerable().Select(grp =>
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
            StartTime = DateTime.Now;
            lblStart.Visibility = Visibility.Visible;
            lblStart.Content = $"Start time: {StartTime}";
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

            EndTime = DateTime.Now;
            lblEnd.Visibility = Visibility.Visible;
            lblEnd.Content = $"End time: {EndTime}";

            //var minutes = (EndTime.Subtract(StartTime).TotalMinutes);

            //lblTotalTime.Content = $"Total minutes tracked: {minutes}";

            var difference = EndTime - StartTime;
            lblTotalTime.Content = ($"Total Time: {difference.Hours:00}:{difference.Minutes:00}:{difference.Seconds + difference.Milliseconds / 1000.0:00}");

            lblTotalTime.Visibility = Visibility.Visible;
        }

        private void btnResetTime_Click(object sender, RoutedEventArgs e)
        {
            lblStart.Visibility = Visibility.Hidden;
            lblEnd.Visibility = Visibility.Hidden;
            lblError.Visibility = Visibility.Hidden;
            lblTotalTime.Visibility = Visibility.Hidden;

            StartTime = DateTime.MinValue;
        }
    }
}
