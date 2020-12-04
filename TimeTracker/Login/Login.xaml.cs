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

namespace TimeTracker.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private LoginLogic loginLogic = new LoginLogic();
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Brings the user to a page where they can sign up for a new account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUp.SignUp signUp = new SignUp.SignUp(); //create your new form.
            signUp.Show();
            this.Close(); //only if you want to close the current form.
        }

        /// <summary>
        /// Will call the function to check the username/password against the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //Set to true if the username and password match what's in the database
            bool isInfoCorrect = false;

            isInfoCorrect = loginLogic.CheckUserInfo(txtUsername.Text, txtPassword.Text);

            //If the info matches, navigate to the next page
            if (isInfoCorrect)
            {
                Input.Input input = new Input.Input(txtUsername.Text);
                input.Show();
                this.Close();
            }
            else
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Content = "Error: username or password is incorrect";
                txtUsername.Text = "Username";
                txtPassword.Text = "Password";
                txtUsername.GotFocus += txtUsername_GotFocus;
                txtPassword.GotFocus += txtPassword_GotFocus;
            }

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
        }

        private void btnBypass_Click(object sender, RoutedEventArgs e)
        {
            SignUp.SignUp signUp = new SignUp.SignUp(); //create your new form.
        }
    }
}
