using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using XmlTableEditor.Data;
using XmlTableEditor.Helpers;
using Microsoft.Win32;
using System.Windows;

namespace XmlTableEditor.ViewModel
{
    class ViewModelMain : ViewModelBase
    {
        string CurrentFilePath { get; set; }

        bool _IsFileOpened;
        public bool IsFileOpened
        {
            get => _IsFileOpened;
            set
            {
                _IsFileOpened = value;
                RaisePropertyChanged("IsFileOpened");
            }
        }

        bool _HasTarget;
        public bool HasTarget
        {
            get => _HasTarget;
            set
            {
                _HasTarget = value;
                RaisePropertyChanged("HasTarget");
            }
        }

        object _SelectedStudent;
        public object SelectedStudent
        {
            get => _SelectedStudent;
            set
            {
                if (_SelectedStudent != value)
                {
                    _SelectedStudent = value;
                    RaisePropertyChanged("SelectedStudent");

                    HasTarget = value != null;
                }
            }
        }

        public ObservableCollection<ViewModelStudent> Students { get; set; }

        public RelayCommand AddRowCommand { get; set; }
        public RelayCommand DelRowCommand { get; set; }

        public RelayCommand OpenFileCommand { get; set; }
        public RelayCommand SaveFileCommand { get; set; }

        public ViewModelMain()
        {
            IsFileOpened = false;
            HasTarget = false;

            AddRowCommand = new RelayCommand(AddRow);
            DelRowCommand = new RelayCommand(DelRow);

            OpenFileCommand = new RelayCommand(OpenFile);
            SaveFileCommand = new RelayCommand(SaveFile);
        }

        void OpenFile(object parameter)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "XML|*.xml";

            if (dialog.ShowDialog() == true)
            {
                CurrentFilePath = dialog.FileName;
                var viewStudents = Data.Students.LoadFile(CurrentFilePath).Select(s => new ViewModelStudent(s));
                Students = new ObservableCollection<ViewModelStudent>(viewStudents);
                RaisePropertyChanged("Students");

                HasTarget = Students.Count > 0;
                IsFileOpened = true;
            }
        }

        void SaveFile(object parameter)
        {
            var modelStudents = Students.Select(s => s.Model);
            Data.Students.SaveFile(CurrentFilePath, modelStudents);
        }

        void AddRow(object parameter)
        {
            Students.Add(
                new ViewModelStudent(
                    new Student()));
            RaisePropertyChanged("Students");
        }

        void DelRow(object parameter)
        {
            if (parameter is ViewModelStudent student)
            {
                if (MessageBox.Show("Удалить строку?", "Удаление строки", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Students.Remove(student);
                    RaisePropertyChanged("Students");

                    HasTarget = Students.Count > 0;
                }
            }
        }
    }
}
