using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace TermProject
{
    sealed class ContactManager
    {
        private static int _count = 0;

        private readonly Dictionary<int, Contact> _contacts;

        private static readonly Lazy<ContactManager> LazyInstance = new Lazy<ContactManager>(() => new ContactManager());

        public static ContactManager Instance => LazyInstance.Value;

        private ContactManager()
        {
            _contacts = new Dictionary<int, Contact>();
        }

        public int AddContact(Contact contact)
        {
            _contacts.Add(++_count, contact);
            return _count;
        }

        public bool RemoveContact(int id)
        {
            try
            {
                _contacts.Remove(id);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.WriteLine(e.StackTrace);
                return false;
            }

            return true;
        }

        public void ImportFromFile(Stream stream)
        {
            using (var parser = new TextFieldParser(stream))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                while(!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (fields != null)
                        AddContact(new Contact(fields[0], fields[2], fields.Length < 3 ? null : fields[3]));
                }
            }
        }

        public void ExportToFile(string path)
        {
            using (var sw = new StreamWriter(path))
            {
                foreach (var v in _contacts.Select(kvp => kvp.Value))
                {
                    sw.WriteLine($"{v.FirstName},{v.LastName}{(v.PhoneNumber == null ? "" : $",{v.PhoneNumber}")}");
                    sw.Flush();
                }
            }
        }
    }
}
