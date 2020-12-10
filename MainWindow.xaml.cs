using System;
using System.Collections.Generic;
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

<<<<<<< Updated upstream
            DBUtils.Instance.Create(new Contact("alpha","bravo", "charlie"));
            DBUtils.Instance.ReadAll().ForEach(n => Trace.WriteLine(n));
=======
            DBUtils.Instance.ReadAll().ForEach(ContactManager.Instance.AddContact);

            ContactsList.ItemsSource = ContactManager.Instance.Contacts;
        }

        private void Edit(object sender, RoutedEventArgs e)
        {

            long.TryParse(((Button) sender).DataContext.ToString(), out long id);

            Contact contact = ContactManager.Instance.FindById(id);

            UpdateWindow updateWindow = new UpdateWindow(contact);
            updateWindow.Show();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.Show();
        }


        private void Delete(object sender, RoutedEventArgs e)
        {

            long.TryParse(((Button) sender).DataContext.ToString(), out var id);

            try
            {
                DBUtils.Instance.RemoveOne(id);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong when trying to delete.\nContact support.", "Delete failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            ContactManager.Instance.RemoveContact(id);
        }

        private void ImportCSV(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openCSV = new OpenFileDialog {DefaultExt = ".csv", Filter = "CSV Files (*.csv)|*.csv"};


            openCSV.FileOk += (_, __) =>
            {
                try
                {
                    List<Contact> toAdd = CSVUtils.Instance.ImportFromFile(openCSV.OpenFile());

                    DBUtils.Instance.CreateMany(toAdd)
                        .ForEach(ContactManager.Instance.AddContact);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong when trying to open your CSV file.\nContact support.",
                        "Import failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    Trace.WriteLine(ex.StackTrace);
                }

            };

            openCSV.ShowDialog();

        }

        private void ExportCSV(object sender, RoutedEventArgs e)
        {
            var exportCSV = new SaveFileDialog { DefaultExt = ".csv", Filter = @"CSV Files (*.csv)|*.csv" };

            exportCSV.FileOk += (_, __) =>
            {
                try
                {
                    CSVUtils.Instance.ExportToFile(exportCSV.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong when trying to export your CSV file.\nContact support.",
                        "Export failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    Trace.WriteLine(ex.StackTrace);
                }
            };


            exportCSV.ShowDialog();

>>>>>>> Stashed changes
        }
    }
}
