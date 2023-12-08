using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DermacoSkinClinic
{
    public partial class AppointmentForm : Window
    {
        public ObservableCollection<Appointment> Appointments { get; set; }

        public AppointmentForm()
        {
            InitializeComponent();
            Appointments = new ObservableCollection<Appointment>();
            AppointmentDataGrid.ItemsSource = Appointments;
        }

        private void SubmitAppointment_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string email = EmailTextBox.Text;
            string phone = PhoneTextBox.Text;
            string Comments = CommentsTextBox.Text;
            string selectedTime = AppointmentTimeComboBox.Text
            // Validate first name
            if (string.IsNullOrWhiteSpace(firstName) || !IsValidName(firstName))
            {
                MessageBox.Show("Please enter a valid first name.", "Invalid First Name", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate last name
            if (string.IsNullOrWhiteSpace(lastName) || !IsValidName(lastName))
            {
                MessageBox.Show("Please enter a valid last name.", "Invalid Last Name", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Validate email
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Please enter a valid Canadian phone number.", "Invalid Phone Number", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!TermsCheckBox.IsChecked.GetValueOrDefault())
            {
                MessageBox.Show("Please accept the terms and conditions.", "Terms and Conditions", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
                //ComboBoxItem selectedItem = AppointmentTimeComboBox.SelectedItem as ComboBoxItem;

                //  DateTime appointDate = AppointmentDateTimePicker.SelectedDate;
                // Continue retrieving data for other controls (LastNameTextBox, EmailTextBox, AppointmentDatePicker, AppointmentTimePicker)

                // Create a new Appointment object with the captured data
                Appointment newAppointment = new Appointment
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Comments = Comments,
                AppointmentDate = (DateTime)AppointmentDateTimePicker.SelectedDate,
                AppointmentTime = selectedTime,
                //AppointmentTime = (TimeSpan)AppointmentTimeComboBox.SelectedItem,
                // Set other properties...
            };


            // Add the new appointment to the ObservableCollection
            Appointments.Add(newAppointment);

            // Clear form fields
            ClearFormFields();
        }

            private bool IsValidEmail(string email)
            {
                try
                {
                    var mailAddress = new System.Net.Mail.MailAddress(email);
                    return mailAddress.Address == email;
                }
                catch
                {
                    return false;
                }
            }
            private bool IsValidName(string name)
            {
                return name.All(char.IsLetter);
            }
            private bool IsValidPhoneNumber(string phoneNumber)
            {
                // Remove non-digit characters from the phone number
                string cleanedNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

                // Validate the cleaned number against the Canadian phone number pattern eg (123) 456-7890
                return System.Text.RegularExpressions.Regex.IsMatch(cleanedNumber, @"^(\d{10}|\(\d{3}\)\s?\d{3}[-\.\s]?\d{4}|\d{3}[-\.\s]?\d{4})$");
            }
                string filePath = "\"C:\\Users\\prana\\Downloads\\DermacoSkinClinic-\\DermacoSkinClinic-finalproject-master\\DermacoSkinClinic-finalproject-master\\DermacoSkinClinic\\Appointments.xml";
            private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the text in the textboxes
            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            PhoneTextBox.Text = string.Empty;
            PostalCodeTextBox.Text = string.Empty;
            CommentsTextBox.Text = string.Empty;
            ProcedureComboBox.SelectedIndex = -1;
            EmailContactCheckBox.IsChecked = false;
            PhoneContactCheckBox.IsChecked = false;
            DOBDatePicker.SelectedDate = null;
            AppointmentTimeComboBox.SelectedIndex = -1;
            EmailSubscriptionCheckBox.IsChecked = false;
            TermsCheckBox.IsChecked = false;
            AppointmentDateTimePicker.SelectedDate = null;
            // EmailSubscriptionCheckBox.IsEnabled; 
            //TermsCheckBox.
            //  AppointmentDataGrid



        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            // Add logic to apply filter to the displayed data
            // Example: Filter appointments based on user input
            string searchText = SearchTextBox.Text;
            string filterOption = FilterComboBox.SelectedItem?.ToString();

            if (filterOption == "Filter by Name")
            {
                var filteredAppointments = Appointments.Where(appointment =>
                    appointment.FirstName.Contains(searchText) || appointment.LastName.Contains(searchText)).ToList();

                AppointmentDataGrid.ItemsSource = filteredAppointments;
            }
            else if (filterOption == "Filter by Email")
            {
                var filteredAppointments = Appointments.Where(appointment =>
                    appointment.Email.Contains(searchText)).ToList();

                AppointmentDataGrid.ItemsSource = filteredAppointments;
            }
            else
            {
                // Reset the filter if no valid filter option is selected
                AppointmentDataGrid.ItemsSource = Appointments;
            }
        }

        private void ClearFormFields()
        {
            FirstNameTextBox.Clear();
            LastNameTextBox.Clear();
            EmailTextBox.Clear();
            PhoneTextBox.Clear();
            PostalCodeTextBox.Clear();
            DOBDatePicker.SelectedDate = null;
            EmailContactCheckBox.IsChecked = false;
            ProcedureComboBox.SelectedIndex = -1;
            // Clear other form fields...
            AppointmentDateTimePicker.SelectedDate = null;
            AppointmentTimeComboBox.SelectedIndex = -1;
            CommentsTextBox.Clear();
            EmailSubscriptionCheckBox.IsChecked = false;
            TermsCheckBox.IsChecked = false;
        }

        private void AppointmentTimeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TermsCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TermsCheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }
    }

    public class Appointment
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public bool IsEmailSubscriptionEnabled { get; set; }
        public bool IsTermsAccepted { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }
}

