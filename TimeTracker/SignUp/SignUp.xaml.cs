using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TimeTracker.SignUp
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        private SignUpLogic SignUpLogic = new SignUpLogic();
        public SignUp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Will delete the text inside of the username field when a user clicks there
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void txtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= txtUsername_GotFocus;
        }

        /// <summary>
        /// Will delete the text inside of the password field when a user clicks there
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= txtPassword_GotFocus;
        }

        /// <summary>
        /// Will delete the text inside of the group number field when a user clicks there
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void txtGroupNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= txtGroupNumber_GotFocus;
        }

        private void btnSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            SignUpLogic.RegisterUser(txtUsername.Text, txtPassword.Text, txtGroupNumber.Text);
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
