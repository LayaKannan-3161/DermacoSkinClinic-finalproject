using System.Windows;
using System.Windows.Controls;

namespace DermacoSkinClinic
{
    public partial class help : Window
    {
        public help()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the help window
            Close();
        }

        private void ContactComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update the contact information based on the selected contact
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedContact = comboBox.SelectedItem as ComboBoxItem;

            if (selectedContact != null)
            {
                string contactName = selectedContact.Content.ToString();
                string contactEmail = "";
                string contactPhone = "";
                string Designation = "";



                // Assign sample contact details based on the selected contact
                switch (contactName)
                {
                    case "Manager":
                        contactEmail = "Name : Ms.Laya      Email: Laya@dermaco.com";
                        contactPhone = "Phone No: (123) 456-7890";
                          break;
                    case "Facility Supervisor":
                        contactEmail = "Name : Mr.Udhaya    Email: Udhay@dermaco.com";
                        contactPhone = "Phone No: (456) 789-0123";
                         break;
                    case "Office Manager":
                        contactEmail = "Name : Ms.Madhu     Email: Madhu@dermaco.com";
                        contactPhone = "Phone No: (789) 012-3456";
                        
                        break;
                    case "Customer support Manager":
                        contactEmail = "Name : Mr.Pranay    Email : Pranay@dermaco.com";
                        contactPhone = "Phone No: (546) 042-3456";
                       
                        break;
                    case "Clinic Manager":
                        contactEmail = "Name : Ms.Sumathi   Email : Sumi@dermaco.com";
                        contactPhone = "Phone No: (456) 345-9856";
                       break;


                        // Add more cases for additional contacts
                }

                // Update the help text based on the selected contact
                ContactInfoTextBlock.Text = $"Contact Information for {contactName}:\n " +
                                            //$"Phone: {contactPhone}\n" +
                                            $"{contactEmail}\n" +  $"{contactPhone}";
            }
        }
    }
}
