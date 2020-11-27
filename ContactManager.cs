using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace TermProject
{
    sealed class ContactManager
    {

        private Dictionary<int, Contact> _contacts;

        private static readonly Lazy<ContactManager> LazyInstance = new Lazy<ContactManager>(() => new ContactManager());

        public static ContactManager Instance => LazyInstance.Value;

        private ContactManager()
        {
            _contacts = new Dictionary<int, Contact>();
        }

        public void AddContact(int id, Contact contact) => _contacts.Add(id, contact);

        public void RemoveContact(int id) => _contacts.Remove(id);

        public void ImportFromFile(int startingId, Stream stream)
        {
            using (TextFieldParser parser = new TextFieldParser(stream))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                for (int id = startingId; !parser.EndOfData; id++)
                {
                    string[] fields = parser.ReadFields();
                    AddContact(id, new Contact(fields[0], fields[2], fields[3]??));
                }
            }
        }

    }
}
