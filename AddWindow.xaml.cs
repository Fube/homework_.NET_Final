using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContactLibrary;

namespace TermProject
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
        }

        private void _Save(object sender, RoutedEventArgs e)
        {
            Contact toAdd = new Contact(NameText.Text, LastNameText.Text, PhoneText.Text);

            // Basic input validation
            Regex reg = new Regex(@"^([\d]{3}-){2}[\d]{4}$");
            if (!reg.IsMatch(PhoneText.Text) && !PhoneText.Text.Equals(string.Empty))
            {
                MessageBox.Show("The phone number is invalid", "Invalid", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                long id = DBUtils.Instance.Create(toAdd);

                var (_, fname, lname, phone) = toAdd;

                ContactManager.Instance.AddContact(new Contact(id, fname, lname, phone));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong when trying to add.\nContact support.", "Add failed", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine(ex.StackTrace);
                return;
            }

            Close();
        }

        private void _Close(object sender, RoutedEventArgs e) => Close();
    }
}
