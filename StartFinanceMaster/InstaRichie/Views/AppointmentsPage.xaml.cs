using SQLite.Net;
using StartFinance.Models;
using System;
using System.IO;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppointmentsPage : Page
    {

        SQLiteConnection connect; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");
        public AppointmentsPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            connect = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            Results();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        public void Results()
        {
            // Creating table
            connect.CreateTable<Appointment>();

            /// Refresh Data
            var query = connect.Table<Appointment>();
            AppointmentListView.ItemsSource = query.ToList();
        }

        private async void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((AppointmentNameText.Text.ToString() == "") || (AppointmentLocationText.Text.ToString() == "") || (AppointmentDateText.Text.ToString() == "") || (AppointmentStartTimeText.Text.ToString() == "") || (AppointmentEndTimeText.Text.ToString() == ""))
                {
                    MessageDialog dialog = new MessageDialog("Please fill in all fields", "Error!");
                    await dialog.ShowAsync();
                }
                else
                {
                    connect.Insert(new Appointment()
                    {
                        EventName = AppointmentNameText.Text,
                        Location = AppointmentLocationText.Text,
                        EventDate = AppointmentDateText.Text,
                        StartTime = AppointmentStartTimeText.Text,
                        EndTime = AppointmentEndTimeText.Text
                    }); ;
                    ClearFieldsEvent();
                    Results();
                }
            }
            catch (Exception)
            {
                MessageDialog dialog = new MessageDialog("Please fill in all fields", "Error!");
                await dialog.ShowAsync();
            }

        }

        private async void ClearFileds_Click(object sender, RoutedEventArgs e)
        {
            AppointmentNameText.Text = string.Empty;
            AppointmentLocationText.Text = string.Empty;
            AppointmentDateText.Text = string.Empty;
            AppointmentStartTimeText.Text = string.Empty;
            AppointmentEndTimeText.Text = string.Empty;

            MessageDialog ClearDialog = new MessageDialog("Cleared", "information");
            await ClearDialog.ShowAsync();
        }

        private async void DeleteAppt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int AccSelection = ((Appointment)AppointmentListView.SelectedItem).AppointmentID;
                if (AccSelection == 0)
                {
                    MessageDialog dialog = new MessageDialog("Item not selected", "Error!");
                    await dialog.ShowAsync();
                }
                else
                {
                    connect.CreateTable<Appointment>();
                    var query1 = connect.Table<Appointment>();
                    var query3 = connect.Query<Appointment>("DELETE FROM Appointment WHERE AppointmentID ='" + AccSelection + "'");
                    AppointmentListView.ItemsSource = query1.ToList();
                    ClearFieldsEvent();
                    Results();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Item not selected", "Error!");
                await dialog.ShowAsync();
            }
        }
        private async void EditAppt_Click(object sender, RoutedEventArgs e)
        {

            if (AppointmentListView.SelectedItem == null)
            {
                await new MessageDialog("No selection made", "Oops..!").ShowAsync();
                return;
            }
            else
            {
                AppointmentListView.IsEnabled = false;
                AddAppointment.IsEnabled = false;
                DeleteAppointment.IsEnabled = false;
                UpdateButton.IsEnabled = true;
                Appointment editData = (Appointment)AppointmentListView.SelectedItem;
                AppointmentNameText.Text = editData.EventName;
                AppointmentDateText.Text = editData.EventDate;
                AppointmentLocationText.Text = editData.Location;
                AppointmentEndTimeText.Text = editData.EndTime;
                AppointmentStartTimeText.Text = editData.StartTime;
                ButtonsStackPanel.Visibility = Visibility.Visible;
            }

        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppointmentListView.SelectedItem == null)
            {
                await new MessageDialog("No selection made", "Oops..!").ShowAsync();
                return;
            }
            else
            {
                if ((AppointmentNameText.Text.ToString() == "") || (AppointmentLocationText.Text.ToString() == "") || (AppointmentDateText.Text.ToString() == "") || (AppointmentStartTimeText.Text.ToString() == "") || (AppointmentEndTimeText.Text.ToString() == ""))
                {
                    MessageDialog dialog = new MessageDialog("Please fill in all fields", "Error!");
                    await dialog.ShowAsync();
                }
                else
                {
                    Appointment updatedData = (Appointment)AppointmentListView.SelectedItem;
                    updatedData.EventName = AppointmentNameText.Text;
                    updatedData.Location = AppointmentLocationText.Text;
                    updatedData.EventDate = AppointmentDateText.Text;
                    updatedData.StartTime = AppointmentStartTimeText.Text;
                    updatedData.EndTime = AppointmentEndTimeText.Text;
                    connect.Update(updatedData);
                    Results();
                    AppointmentListView.SelectedItem = null;
                    AppointmentListView.IsEnabled = true;
                    AddAppointment.IsEnabled = true;
                    DeleteAppointment.IsEnabled = true;
                    ButtonsStackPanel.Visibility = Visibility.Collapsed;
                    ClearFieldsEvent();
                }
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            AppointmentListView.SelectedItem = null;
            AppointmentListView.IsEnabled = true;
            AddAppointment.IsEnabled = true;
            DeleteAppointment.IsEnabled = true;
            ButtonsStackPanel.Visibility = Visibility.Collapsed;
            ClearFieldsEvent();
        }
        private void ClearFieldsEvent()
        {
            AppointmentNameText.Text = string.Empty;
            AppointmentLocationText.Text = string.Empty;
            AppointmentDateText.Text = string.Empty;
            AppointmentStartTimeText.Text = string.Empty;
            AppointmentEndTimeText.Text = string.Empty;
        }
    }
}
