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

            DBUtils.Instance.Create(new Contact("alpha","bravo", "charlie"));
            DBUtils.Instance.ReadAll().ForEach(n => Trace.WriteLine(n));
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(((Button) e.Source).DataContext);
        }


        private void Delete(object sender, RoutedEventArgs e)
        {

            long.TryParse(((Button) sender).DataContext.ToString(), out long id);

            contacts.Remove(contacts.Single(n => n.ID == id));

            DBUtils.Instance.RemoveOne(id);
        }
    }
}
