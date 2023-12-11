// MainWindow.xaml.cs
using DermacoSkinClinic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DermacoSkinClinic
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement your authentication logic here
            string expectedUsername = "admin";
            string expectedPassword = "admin";


            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Check username and password (example: hardcoded for demonstration)
            if (username != expectedUsername || password != expectedPassword)
            {
                // Wrong entry, highlight the text boxes in red
                HighlightControl(txtUsername);
                HighlightControl(txtPassword);
                MessageBox.Show("Incorrect username or password");
            }
            else
            {
                // Correct entry, proceed with your login logic
                MessageBox.Show("Login successful!");
                AppointmentForm appointmentForm = new AppointmentForm();
                appointmentForm.Show();
                this.Close();
            }
        }
        private void HighlightControl(Control control)
        {
            // Highlight the TextBox with a red border
            control.BorderBrush = Brushes.Red;
            control.BorderThickness = new Thickness(2);
        }



        private void Logincancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
