﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace DermacoSkinClinic
{
    public partial class AppointmentForm : Window
    {
        private readonly AppointmentDataAccess dataAccess;

        public ObservableCollection<Appointment> Appointments { get; set; }

        public AppointmentForm()
        {
            InitializeComponent();
            dataAccess = new AppointmentDataAccess();
            Appointments = dataAccess.LoadAppointments();
            AppointmentDataGrid.ItemsSource = Appointments;
        }

        public void SaveAppointments(ObservableCollection<Appointment> appointments)
        {
            try
            {
                using (var writer = new StreamWriter(AppointmentDataAccess.XmlFilePath))
                {
                    var serializer = new XmlSerializer(typeof(ObservableCollection<Appointment>));
                    serializer.Serialize(writer, appointments);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving appointments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public class AppointmentDataAccess
        {
            public const string XmlFilePath = "Appointments.xml";


            public ObservableCollection<Appointment> LoadAppointments()
            {
                if (File.Exists(XmlFilePath))
                {
                    try
                    {
                        using (var reader = new StreamReader(XmlFilePath))
                        {
                            var serializer = new XmlSerializer(typeof(ObservableCollection<Appointment>));
                            return (ObservableCollection<Appointment>)serializer.Deserialize(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                return new ObservableCollection<Appointment>();
            }

            public void SaveAppointments(ObservableCollection<Appointment> appointments)
            {
                try
                {
                    using (var writer = new StreamWriter(XmlFilePath))
                    {
                        var serializer = new XmlSerializer(typeof(ObservableCollection<Appointment>));
                        serializer.Serialize(writer, appointments);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving appointments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadAppointments()
        {
            if (File.Exists(AppointmentDataAccess.XmlFilePath))
            {
                try
                {
                    using (var reader = new StreamReader(AppointmentDataAccess.XmlFilePath))
                    {
                        var serializer = new XmlSerializer(typeof(ObservableCollection<Appointment>));
                        Appointments = (ObservableCollection<Appointment>)serializer.Deserialize(reader);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void SubmitAppointment_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string email = EmailTextBox.Text;
            string phone = PhoneTextBox.Text;
            string comments = CommentsTextBox.Text;
            string consultantname = ConsultantComboBox.Text;
            string selectedTime = AppointmentTimeComboBox.Text;
            bool hasInsurance = InsuranceCheckBox.IsChecked.GetValueOrDefault();
            string insuranceNumber = InsuranceNumberTextBox.Text;
            string paymentMode = PaymentModeComboBox.SelectedItem?.ToString();
            string creditCardNumber = CreditCardNumberTextBox.Text;

            if (string.IsNullOrWhiteSpace(firstName) || !IsValidName(firstName))
            {
                HighlightInvalidField(FirstNameTextBox);
                MessageBox.Show("Please enter a valid first name.", "Invalid First Name", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate last name
            if (string.IsNullOrWhiteSpace(lastName) || !IsValidName(lastName))
            {
                HighlightInvalidField(LastNameTextBox);
                MessageBox.Show("Please enter a valid last name.", "Invalid Last Name", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate email
            if (!IsValidEmail(email))
            {
                HighlightInvalidField(EmailTextBox);
                MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate phone number
            if (!IsValidPhoneNumber(phone))
            {
                HighlightInvalidField(PhoneTextBox);
                MessageBox.Show("Please enter a valid Canadian phone number.", "Invalid Phone Number", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate credit card number
            if (!IsValidCreditCardNumber(creditCardNumber))
            {
                HighlightInvalidField(CreditCardNumberTextBox);
                MessageBox.Show("Please enter a valid 16-digit numeric credit card number.", "Invalid Credit Card Number", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate insurance number
            if (!IsValidInsuranceNumber(insuranceNumber))
            {
                HighlightInvalidField(InsuranceNumberTextBox);
                MessageBox.Show("Please enter a valid 10-digit numeric insurance number.", "Invalid Insurance Number", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate Canadian postal code
            if (!IsValidCanadianPostalCode(PostalCodeTextBox.Text))
            {
                HighlightInvalidField(PostalCodeTextBox);
                MessageBox.Show("Please enter a valid Canadian postal code.", "Invalid Postal Code", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate terms checkbox
            if (!TermsCheckBox.IsChecked.GetValueOrDefault())
            {
                HighlightInvalidField(TermsCheckBox);
                MessageBox.Show("Please accept the terms and conditions.", "Terms and Conditions", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Appointment newAppointment = new Appointment
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                AppointmentDate = (DateTime)AppointmentDateTimePicker.SelectedDate,
                AppointmentTime = selectedTime,
                ConsultantName = consultantname,
                ProcedureInterested = ProcedureComboBox.SelectedItem?.ToString() ?? string.Empty,
            };
            Appointments.Add(newAppointment);
            // Display user information in the UI
            DisplayUserProfile(newAppointment);

            // Save appointments after adding a new one
            SaveAppointments(Appointments); // Pass the Appointments collection

            // Clear form fields
            ClearFormFields();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected appointment or user information
            //Appointment selectedAppointment = AppointmentDataGrid.SelectedItem as Appointment;

            //if (selectedAppointment != null)
            //{
            UserProfilePage userProfilePage = new UserProfilePage();

            userProfilePage.Show();
            // userProfilePage.UserNameTextBox.Text = $"Name: {selectedAppointment.FirstName} {selectedAppointment.LastName}";
            // userProfilePage.EmailTextBox.Text = $"Email: {selectedAppointment.Email}";
            //  userProfilePage.PhoneTextBox.Text = $"Phone: {selectedAppointment.Phone}";

            this.Close();
            //}
        }

        private void DisplayUserProfile(Appointment appointment)
        {
            // Assuming you have TextBlock controls named FirstNameTextBlock, LastNameTextBlock, PhoneTextBlock, and EmailTextBlock in your XAML
            FirstNameTextBox.Text = appointment.FirstName;
            LastNameTextBox.Text = appointment.LastName;
            PhoneTextBox.Text = appointment.Phone; // Use the appropriate property from your Appointment class
            EmailTextBox.Text = appointment.Email;
        }

        private bool IsValidInsuranceNumber(string insuranceNumber)
        {
            if (string.IsNullOrWhiteSpace(insuranceNumber))
            {
                return false;
            }

            // Check if the insurance number contains only numeric characters
            if (!insuranceNumber.All(char.IsDigit))
            {
                return false;
            }

            // Check if the insurance number is exactly 10 digits long
            return insuranceNumber.Length == 10;
        }

        private bool IsValidCanadianPostalCode(string postalCode)
        {
            if (string.IsNullOrWhiteSpace(postalCode))
            {
                return false;
            }

            // Canadian postal code regex pattern
            string pattern = @"^[ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ][ -]?\d[ABCEGHJKLMNPRSTVWXYZ]\d$";

            return System.Text.RegularExpressions.Regex.IsMatch(postalCode, pattern);
        }

        private bool IsValidCreditCardNumber(string creditCardNumber)
        {
            if (string.IsNullOrWhiteSpace(creditCardNumber))
            {
                return false;
            }

            // Check if the credit card number contains only numeric characters
            if (!creditCardNumber.All(char.IsDigit))
            {
                return false;
            }

            // Check if the credit card number is exactly 16 digits long
            return creditCardNumber.Length == 16;
        }

        private void HighlightInvalidField(Control control)
        {
            // Highlight the invalid field by setting the border color to red
            control.BorderBrush = Brushes.Red;
            control.BorderThickness = new Thickness(2);
        }

        private void ClearInvalidFieldHighlights()
        {
            // Clear highlighting for all fields
            FirstNameTextBox.ClearValue(BorderBrushProperty);
            LastNameTextBox.ClearValue(BorderBrushProperty);
            EmailTextBox.ClearValue(BorderBrushProperty);
            PhoneTextBox.ClearValue(BorderBrushProperty);
            CreditCardNumberTextBox.ClearValue(BorderBrushProperty);
            InsuranceNumberTextBox.ClearValue(BorderBrushProperty);
            PostalCodeTextBox.ClearValue(BorderBrushProperty);
            TermsCheckBox.ClearValue(BorderBrushProperty);
            ConsultantComboBox.ClearValue(BorderBrushProperty);
            // Add other fields as needed
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim();

            // Use SelectedItem directly, no need for ItemsSource or data binding
            if (FilterComboBox.SelectedItem is ComboBoxItem selectedFilterItem)
            {
                string filterOption = selectedFilterItem.Content.ToString();

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
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            // Clear filter logic
            FilterComboBox.SelectedIndex = -1;
            SearchTextBox.Text = string.Empty;

            // Reset the data grid to show all appointments
            AppointmentDataGrid.ItemsSource = null;
            AppointmentDataGrid.ItemsSource = Appointments;
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
            ConsultantComboBox.SelectedIndex = -1;
            InsuranceNumberTextBox.Clear();
            PaymentModeComboBox.SelectedIndex = -1;
            CreditCardNumberTextBox.Clear();
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
            public DateTime AppointmentDate { get; set; } = DateTime.MinValue; // or another default value
            public string AppointmentTime { get; set; }
            public string ProcedureInterested { get; set; } = string.Empty; // Add this property
            public string Phone { get; set; } = string.Empty;
            public string ConsultantName { get; set; } = string.Empty;
        }

        private void LastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to logout?", "Logout Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            // Check the user's choice
            if (result == MessageBoxResult.Yes)
            {
                // Perform logout actions here
                // For simplicity, we'll just close the application
                Application.Current.Shutdown();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Aboutus win = new Aboutus();
            win.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            help hel = new help();
            hel.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
        }

       
    }
    }
