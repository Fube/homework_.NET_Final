using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using ContactLibrary;

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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void _Save(object sender, RoutedEventArgs e)
        {
            ContactManager.Instance.UpdateContact(
                new Contact(contact.ID, NameText.Text, LastNameText.Text, PhoneText.Text)
            );

            DBUtils.Instance.Update(contact);

            _Close(null, null);
        }

        private void _Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
