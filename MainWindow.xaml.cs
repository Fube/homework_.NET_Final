using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using ContactLibrary;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

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
            using (var exportCSV = new FolderBrowserDialog())
            {

                var result = exportCSV.ShowDialog();

                if (result != System.Windows.Forms.DialogResult.OK) return;

                try
                {
                    CSVUtils.Instance.ExportToFile(exportCSV.SelectedPath + @"\contacts.csv");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong when trying to export your CSV file.\nContact support.",
                        "Export failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    Trace.WriteLine(ex.StackTrace);
                }
                
            }
        }
    }
}
