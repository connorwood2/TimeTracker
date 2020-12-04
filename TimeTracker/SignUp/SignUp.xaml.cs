using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Data;

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

            //Populate the combo box
            DataSet groupNums = SignUpLogic.GetAllGroupNumbers();
            for (int i = 0; i < groupNums.Tables[0].Rows.Count; i++)
            {
                //Observers should not be able to be chosen
                if (groupNums.Tables[0].Rows[i][0].ToString() == "0")
                {
                    continue;
                }
                cbxGroupSelection.Items.Add(groupNums.Tables[0].Rows[i][0].ToString());
            }
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

        private void cbxGroupSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtGroup.Text = "Group Number";
        }
        private void txtGroup_KeyUp(object sender, KeyEventArgs e)
        {
            var groupNum = txtGroup.Text;
            //When I change these, it triggers the function for a cbxGroupSelection change and sets the txtGroup back to 0. 
            //That's why I need to reset the txtGroup.Text to the groupNum variable.
            cbxGroupSelection.SelectedIndex = -1;
            cbxGroupSelection.SelectedItem = null;
            txtGroup.Text = groupNum;
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

        /// <summary>
        /// Will delete the text inside of the group field when a user clicks there
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void txtGroup_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
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
            if ((txtGroup.Text == "" || txtGroup.Text == "Group") && cbxGroupSelection.SelectedItem == null)
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Content = "Error: Missing required field: Group Number";
                return;
            }

            //We know that this point that either the txtGroup or cbxGroupSelection is filled in. Now we need to find out which one it is.
            string groupNum = "";
            if(cbxGroupSelection.SelectedItem == null)
            {
                groupNum = txtGroup.Text;
            }
            else
            {
                groupNum = cbxGroupSelection.SelectedItem.ToString();
            }
            //Try and insert it. If it inserts properly, it will return false. If it can't insert (only because the username is already taken), then it will return true. Odd, yes I know.
            bool usernameAlreadyExists = SignUpLogic.RegisterUser(txtUsername.Text, txtPassword.Text, groupNum);
            if (usernameAlreadyExists)
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Content = "Error: Username already exists - Try again";
                txtUsername.Text = "Try Again";
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
