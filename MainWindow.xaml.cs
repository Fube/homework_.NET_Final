using System;
using System.Collections.Generic;
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

namespace TermProject
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<Contacts> person = new List<Contacts>();
            person.Add(new Contacts() { FirstName = "Shariful", LastName = "Islam", PhoneNumber = "514-239-2349" });
            person.Add(new Contacts() { FirstName = "Christian", LastName = "Chitanu", PhoneNumber = "514-232-4324" });
            person.Add(new Contacts() { FirstName = "Nariman", LastName = "Abrari", PhoneNumber = "514-334-3432" });
            Contact.ItemsSource = person;
        }
    }

    public class Contacts
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

    }
}