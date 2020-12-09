using ContactLibrary;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace TermProject
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private Contact contact;
        public UpdateWindow(Contact contact)
        {
            this.contact = contact;
            InitializeComponent();

            NameText.Text = contact.FirstName;
            LastNameText.Text = contact.LastName;
            PhoneText.Text = contact.PhoneNumber;
        }

        private void _Save(object sender, RoutedEventArgs e)
        {

            // For rollback purposes
            Contact oldContact = (Contact)contact.Clone();


            // Basic input validation
            Regex reg = new Regex(@"[\d-]+");
            if (!reg.IsMatch(PhoneText.Text) && !PhoneText.Text.Equals(string.Empty))
            {
                MessageBox.Show("The phone number is invalid", "Invalid", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // This must run before the DB update, because the DB updated is based on this mutation
            ContactManager.Instance.UpdateContact(
                new Contact(contact.ID, NameText.Text, LastNameText.Text, PhoneText.Text)
            );

            try
            {
                DBUtils.Instance.Update(contact);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong when trying to edit.\nContact support.", "Update failed", MessageBoxButton.OK, MessageBoxImage.Error);
                ContactManager.Instance.UpdateContact(oldContact);
                return;
            }

            _Close(null, null);
        }

        private void _Close(object sender, RoutedEventArgs e) => Close();
    }
}
