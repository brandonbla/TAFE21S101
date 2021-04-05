using SQLite.Net;
using StartFinance.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PersonalInfo : Page
    {
        SQLiteConnection connect; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public PersonalInfo()
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
            connect.CreateTable<Person>();

            /// Refresh Data
            var query = connect.Table<Person>();
            PersonalListView.ItemsSource = query.ToList();
        }

        private async void AddPersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((PersonalFirstNameText.Text.ToString() == "") || (PersonalLastNameText.Text.ToString() == "") || (DOBText.Text.ToString() == "") || (GenderText.Text.ToString() == "") || (PersonalEmailText.Text.ToString() == "") || (PersonalPhoneText.Text.ToString() == ""))
                {
                    MessageDialog dialog = new MessageDialog("Please fill in all fields", "Error!");
                    await dialog.ShowAsync();
                }
                else
                {
                    connect.Insert(new Person()
                    {
                        PersonalFirstName = PersonalFirstNameText.Text,
                        PersonalLastName = PersonalLastNameText.Text,
                        DOB = DOBText.Text,
                        Gender = GenderText.Text,
                        PersonalEmail = PersonalEmailText.Text,
                        PersonalPhone = PersonalPhoneText.Text
                    });
                    ClearFieldsEvent();
                    Results();
                }
            }
            catch (Exception ex)
            {
                MessageDialog dialog = new MessageDialog("Please fill in all fields", "Error!");
                await dialog.ShowAsync();
            }
            
        }

        private async void ClearFileds_Click(object sender, RoutedEventArgs e)
        {
            PersonalFirstNameText.Text = string.Empty;
            PersonalLastNameText.Text = string.Empty;
            DOBText.Text = string.Empty;
            GenderText.Text = string.Empty;
            PersonalEmailText.Text = string.Empty;
            PersonalPhoneText.Text = string.Empty;

            MessageDialog ClearDialog = new MessageDialog("Cleared", "information");
            await ClearDialog.ShowAsync();
        }

        private async void DeletePersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int AccSelection = ((Person)PersonalListView.SelectedItem).PersonalID;
                if (AccSelection == 0)
                {
                    MessageDialog dialog = new MessageDialog("Item not selected", "Error!");
                    await dialog.ShowAsync();
                }
                else
                {
                    connect.CreateTable<Person>();
                    var query1 = connect.Table<Person>();
                    var query3 = connect.Query<Person>("DELETE FROM Person WHERE PersonalID ='" + AccSelection + "'");
                    PersonalListView.ItemsSource = query1.ToList();
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
        private async void EditPersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            
            if (PersonalListView.SelectedItem == null)
            {
                await new MessageDialog("No selection made", "Oops..!").ShowAsync();
                return;
            }
            else
            {
                PersonalListView.IsEnabled = false;
                AddPersonalInfo.IsEnabled = false;
                DeletePersonalInfo.IsEnabled = false;
                UpdateButton.IsEnabled = true;
                Person editData = (Person)PersonalListView.SelectedItem;
                PersonalFirstNameText.Text = editData.PersonalFirstName;
                PersonalLastNameText.Text = editData.PersonalLastName;
                DOBText.Text = editData.DOB;
                GenderText.Text = editData.Gender;
                PersonalEmailText.Text = editData.PersonalEmail;
                PersonalPhoneText.Text = editData.PersonalPhone;
                ButtonsStackPanel.Visibility = Visibility.Visible;
            }

        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (PersonalListView.SelectedItem == null)
            {
                await new MessageDialog("No selection made", "Oops..!").ShowAsync();
                return;
            }
            else
            {
                if ((PersonalFirstNameText.Text.ToString() == "") || (PersonalLastNameText.Text.ToString() == "") || (DOBText.Text.ToString() == "") || (GenderText.Text.ToString() == "") || (PersonalEmailText.Text.ToString() == "") || (PersonalPhoneText.Text.ToString() == ""))
                {
                    MessageDialog dialog = new MessageDialog("Please fill in all fields", "Error!");
                    await dialog.ShowAsync();
                }
                else { 
                Person updatedData = (Person)PersonalListView.SelectedItem;
                    updatedData.PersonalFirstName = PersonalFirstNameText.Text;
                    updatedData.PersonalLastName = PersonalLastNameText.Text;
                    updatedData.DOB = DOBText.Text;
                    updatedData.Gender = GenderText.Text;
                    updatedData.PersonalEmail = PersonalEmailText.Text;
                    updatedData.PersonalPhone = PersonalPhoneText.Text;
                    connect.Update(updatedData);
                    Results();
                    PersonalListView.SelectedItem = null;
                    PersonalListView.IsEnabled = true;
                    AddPersonalInfo.IsEnabled = true;
                    DeletePersonalInfo.IsEnabled = true;
                    ButtonsStackPanel.Visibility = Visibility.Collapsed;
                    ClearFieldsEvent();
                }
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            PersonalListView.SelectedItem = null;
            PersonalListView.IsEnabled = true;
            AddPersonalInfo.IsEnabled = true;
            DeletePersonalInfo.IsEnabled = true;
            ButtonsStackPanel.Visibility = Visibility.Collapsed;
            ClearFieldsEvent();
        }
        private void ClearFieldsEvent()
        {
            PersonalFirstNameText.Text = string.Empty;
            PersonalLastNameText.Text = string.Empty;
            DOBText.Text = string.Empty;
            GenderText.Text = string.Empty;
            PersonalEmailText.Text = string.Empty;
            PersonalPhoneText.Text = string.Empty;
        }
    }
}
