using System.Windows;
using DermacoSkinClinic; // Add this line at the beginning of your file


namespace DermacoSkinClinic
{
    public partial class UserProfilePage : Window
    {
        public UserProfilePage()
        {
            InitializeComponent();
        }

        // Properties to access UI elements in UserProfilePage.xaml

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            
           AppointmentForm frm = new AppointmentForm();
            frm.Show();
            Close();
         }
    }
}
