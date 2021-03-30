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
    public sealed partial class ContactDetails : Page
    {
        SQLiteConnection connect; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public ContactDetails()
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
            connect.CreateTable<Contact>();

            /// Refresh Data
            var query = connect.Table<Contact>();
            ContactListView.ItemsSource = query.ToList();
        }

        private async void AddContact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((ContactFirstNameText.Text.ToString() == "") || (ContactLastNameText.Text.ToString() == "") || (ContactCompanyText.Text.ToString() == "") || (ContactPhoneText.Text.ToString() == ""))
                {
                    MessageDialog dialog = new MessageDialog("Please fill in all fields", "Error!");
                    await dialog.ShowAsync();
                }
                else
                {
                    connect.Insert(new Contact()
                    {
                        ContactFirstName = ContactFirstNameText.Text,
                        ContactLastName = ContactLastNameText.Text,
                        ContactCompany = ContactCompanyText.Text,
                        ContactPhone = ContactPhoneText.Text
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
            ContactFirstNameText.Text = string.Empty;
            ContactLastNameText.Text = string.Empty;
            ContactCompanyText.Text = string.Empty;
            ContactPhoneText.Text = string.Empty;

            MessageDialog ClearDialog = new MessageDialog("Cleared", "information");
            await ClearDialog.ShowAsync();
        }

        private async void DeleteContact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int AccSelection = ((Contact)ContactListView.SelectedItem).ContactID;
                if (AccSelection == 0)
                {
                    MessageDialog dialog = new MessageDialog("Item not selected", "Error!");
                    await dialog.ShowAsync();
                }
                else
                {
                    connect.CreateTable<Contact>();
                    var query1 = connect.Table<Contact>();
                    var query3 = connect.Query<Contact>("DELETE FROM Contact WHERE ContactID ='" + AccSelection + "'");
                    ContactListView.ItemsSource = query1.ToList();
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
        private async void EditContact_Click(object sender, RoutedEventArgs e)
        {
            
            if (ContactListView.SelectedItem == null)
            {
                await new MessageDialog("No selection made", "Oops..!").ShowAsync();
                return;
            }
            else
            {
                ContactListView.IsEnabled = false;
                AddContact.IsEnabled = false;
                DeleteContact.IsEnabled = false;
                UpdateButton.IsEnabled = true;
                Contact editData = (Contact)ContactListView.SelectedItem;
                ContactFirstNameText.Text = editData.ContactFirstName;
                ContactLastNameText.Text = editData.ContactLastName;
                ContactCompanyText.Text = editData.ContactCompany;
                ContactPhoneText.Text = editData.ContactPhone;
                ButtonsStackPanel.Visibility = Visibility.Visible;
            }

        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContactListView.SelectedItem == null)
            {
                await new MessageDialog("No selection made", "Oops..!").ShowAsync();
                return;
            }
            else
            {
                if ((ContactFirstNameText.Text.ToString() == "") || (ContactLastNameText.Text.ToString() == "") || (ContactCompanyText.Text.ToString() == "") || (ContactPhoneText.Text.ToString() == ""))
                {
                    MessageDialog dialog = new MessageDialog("Please fill in all fields", "Error!");
                    await dialog.ShowAsync();
                }
                else { 
                Contact updatedData = (Contact)ContactListView.SelectedItem;
                    updatedData.ContactFirstName = ContactFirstNameText.Text;
                    updatedData.ContactLastName = ContactLastNameText.Text;
                    updatedData.ContactCompany = ContactCompanyText.Text;
                    updatedData.ContactPhone = ContactPhoneText.Text;
                    connect.Update(updatedData);
                    Results();
                    ContactListView.SelectedItem = null;
                    ContactListView.IsEnabled = true;
                    AddContact.IsEnabled = true;
                    DeleteContact.IsEnabled = true;
                    ButtonsStackPanel.Visibility = Visibility.Collapsed;
                    ClearFieldsEvent();
                }
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ContactListView.SelectedItem = null;
            ContactListView.IsEnabled = true;
            AddContact.IsEnabled = true;
            DeleteContact.IsEnabled = true;
            ButtonsStackPanel.Visibility = Visibility.Collapsed;
            ClearFieldsEvent();
        }
        private void ClearFieldsEvent()
        {
            ContactFirstNameText.Text = string.Empty;
            ContactLastNameText.Text = string.Empty;
            ContactCompanyText.Text = string.Empty;
            ContactPhoneText.Text = string.Empty;
        }

    }
}
