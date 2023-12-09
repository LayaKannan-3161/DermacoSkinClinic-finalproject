// MainWindow.xaml.cs
using DermacoSkinClinic;
using System.Windows;

namespace YourNamespace
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
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Check username and password (example: hardcoded for demonstration)
            if (username == "admin" && password == "admin")
            {
                // Navigate to the next page (MainWindow2 for example)
                AppointmentForm appointmentForm = new AppointmentForm();
                appointmentForm.Show();

                // Close the current login window
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private void Logincancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}


