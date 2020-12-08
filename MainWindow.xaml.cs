using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContactLibrary;

namespace TermProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            DBUtils.Instance.ReadAll().ForEach(ContactManager.Instance.AddContact);

            ContactsList.ItemsSource = ContactManager.Instance.Contacts;
        }

        private void Edit(object sender, RoutedEventArgs e)
        {

            long.TryParse(((Button)sender).DataContext.ToString(), out long id);

            Contact contact = ContactManager.Instance.FindById(id);

            UpdateWindow updateWindow = new UpdateWindow(contact);
            updateWindow.Show();
        }

        private void Add(object sender, RoutedEventArgs e)
        {

        }


        private void Delete(object sender, RoutedEventArgs e)
        {

            long.TryParse(((Button) sender).DataContext.ToString(), out long id);

            try
            {
                DBUtils.Instance.RemoveOne(id);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong when trying to delete.\nContact support.", "Delete failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            

            ContactManager.Instance.RemoveContact(id);
        }
    }
}
