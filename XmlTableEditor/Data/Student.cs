using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace XmlTableEditor.Data
{
    public class Student : INotifyPropertyChanged
    {
        private static int lastID = 0;
        private static HashSet<int> IDs = new HashSet<int>();

        public static void ResetPoolID(IEnumerable<int> ids)
        {
            lastID = 0;
            IDs.Clear();
            IDs.UnionWith(ids);
        }

        public Student()
        {
            do
            {
                ID = lastID++;
            } while (IDs.Add(ID) == false);
        }

        int _ID;
        [XmlAttribute("Id")]
        public int ID
        {
            get => _ID;
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    RaisePropertyChanged("ID");
                }
            }
        }

        string _FirstName;
        public string FirstName
        {
            get => _FirstName;
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    RaisePropertyChanged("FirstName");
                }
            }
        }

        string _Last;
        public string Last
        {
            get => _Last;
            set
            {
                if (_Last != value)
                {
                    _Last = value;
                    RaisePropertyChanged("LastName");
                }
            }
        }

        string _Age;
        public string Age
        {
            get => _Age;
            set
            {
                if (_Age != value)
                {
                    _Age = value;
                    RaisePropertyChanged("Age");
                }
            }
        }

        int _Gender;
        public int Gender
        {
            get => _Gender;
            set
            {
                if (_Gender != value)
                {
                    _Gender = value;
                    RaisePropertyChanged("Gender");
                }
            }
        }

        void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
