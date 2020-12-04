﻿using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;

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
        /// Will only allow numbers to be inputted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
        /// Will delete the text inside of the group field when a user clicks there
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void txtGroup_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= txtGroup_GotFocus;
        }

        private void btnSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == "" || txtUsername.Text == "Username")
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Content = "Error: Missing required field: Username";
                return;
            }
            if (txtPassword.Text == "" || txtPassword.Text == "Password")
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Content = "Error: Missing required field: Password";
                return;
            }
            if (txtGroup.Text == "" || txtGroup.Text == "Group")
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Content = "Error: Missing required field: Group";
                return;
            }

            bool usernameAlreadyExists = SignUpLogic.RegisterUser(txtUsername.Text, txtPassword.Text, txtGroup.Text);
            if (usernameAlreadyExists)
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Content = "Error: Username already exists - Try again";
                txtUsername.Text = "Username";
                txtUsername.GotFocus += txtUsername_GotFocus;
                return;
            }
            else
            {
                Login.Login login = new Login.Login();
                login.Show();
                this.Close();
                return;
            }
        }

        private void btnBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            Login.Login login = new Login.Login();
            login.Show();
            Close();
            return;
        }
    }
}
