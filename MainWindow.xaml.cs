using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ContactLibrary;
using Microsoft.Win32;

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
                MessageBox.Show("Something went wrong when trying to delete.\nContact support.", "Delete failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            

            ContactManager.Instance.RemoveContact(id);
        }

        private void ImportCSV(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openCSV = new OpenFileDialog {DefaultExt = ".csv", Filter = "CSV Files (*.csv)|*.csv"};


            bool? result = openCSV.ShowDialog();

            if (result == false)
            {
                MessageBox.Show("Something went wrong when trying to open your CSV file.\nContact support.", "Import failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string fileName = openCSV.FileName;

            List<Contact> toAdd = CSVUtils.Instance.ImportFromFile(new StreamReader(fileName).BaseStream);

            Console.WriteLine(toAdd.Count);
           DBUtils.Instance.CreateMany(toAdd)
               .ForEach(ContactManager.Instance.AddContact);

        }

        private void ExportCSV(object sender, RoutedEventArgs e)
        {
        }
    }
}
