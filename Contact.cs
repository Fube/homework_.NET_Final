using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermProject
{
    class Contact
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public Contact(string fName, string lName, string phone)
        {
            FirstName = fName;
            LastName = lName;
            PhoneNumber = phone;
        }

        public Contact(string fName, string lName) : this(fName, lName, null){}

        public override string ToString() => $"Name: {FirstName} {LastName}\nPhone Number: {PhoneNumber}";

        public void Deconstruct(out string fName, out string lName, out string phone)
        {
            fName = FirstName;
            lName = LastName;
            phone = PhoneNumber;
        }
    }
}
