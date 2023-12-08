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
            string comments = CommentsTextBox.Text;
            string selectedTime = AppointmentTimeComboBox.Text;

            if (string.IsNullOrWhiteSpace(firstName) || !IsValidName(firstName))
            {
                MessageBox.Show("Please enter a valid first name.", "Invalid First Name", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(lastName) || !IsValidName(lastName))
            {
                MessageBox.Show("Please enter a valid last name.", "Invalid Last Name", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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
            }

            Appointment newAppointment = new Appointment
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Comments = comments,
                AppointmentDate = (DateTime)AppointmentDateTimePicker.SelectedDate,
                AppointmentTime = selectedTime,
            };

            Appointments.Add(newAppointment);

            ClearFormFields();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFormFields();
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
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
                AppointmentDataGrid.ItemsSource = Appointments;
            }
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            // Clear filter logic, if needed
            AppointmentDataGrid.ItemsSource = Appointments;
            FilterComboBox.SelectedIndex = -1;
            SearchTextBox.Text = string.Empty;
        }

        private void TermsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Handle terms checkbox checked event, if needed
        }

        private void TermsCheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            // Handle terms checkbox checked event, if needed
        }

        private void EmailSubscriptionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Handle email subscription checkbox checked event, if needed
        }

        private void ClearFormFields()
        {
            FirstNameTextBox.Clear();
            LastNameTextBox.Clear();
            EmailTextBox.Clear();
            PhoneTextBox.Clear();
            PostalCodeTextBox.Clear();
            CommentsTextBox.Clear();
            ProcedureComboBox.SelectedIndex = -1;
            EmailContactCheckBox.IsChecked = false;
            PhoneContactCheckBox.IsChecked = false;
            DOBDatePicker.SelectedDate = null;
            AppointmentTimeComboBox.SelectedIndex = -1;
            EmailSubscriptionCheckBox.IsChecked = false;
            TermsCheckBox.IsChecked = false;
            AppointmentDateTimePicker.SelectedDate = null;
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
            string cleanedNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());
            return System.Text.RegularExpressions.Regex.IsMatch(cleanedNumber, @"^(\d{10}|\(\d{3}\)\s?\d{3}[-\.\s]?\d{4}|\d{3}[-\.\s]?\d{4})$");
        }

        public class Appointment
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Comments { get; set; } = string.Empty;
            public DateTime AppointmentDate { get; set; }
            public string AppointmentTime { get; set; }
        }
    }
}
