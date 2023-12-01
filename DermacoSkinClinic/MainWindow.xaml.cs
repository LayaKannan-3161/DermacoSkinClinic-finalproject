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
            // Continue retrieving data for other controls (LastNameTextBox, EmailTextBox, AppointmentDatePicker, AppointmentTimePicker)

            // Create a new Appointment object with the captured data
            Appointment newAppointment = new Appointment
            {
                FirstName = firstName,
                // Set other properties...
            };

            // Add the new appointment to the ObservableCollection
            Appointments.Add(newAppointment);

            // Clear form fields
            ClearFormFields();
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
            // Clear other form fields...
            AppointmentDateTimePicker.SelectedDate = null;
            AppointmentTimeComboBox.SelectedIndex = -1;
            CommentsTextBox.Clear();
            EmailSubscriptionCheckBox.IsChecked = false;
            TermsCheckBox.IsChecked = false;
        }
    }

    public class Appointment
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public bool IsEmailSubscriptionEnabled { get; set; }
        public bool IsTermsAccepted { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }
}
