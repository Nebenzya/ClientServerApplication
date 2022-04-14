using System.ComponentModel;

namespace ClientApp
{
    [System.Serializable]
    public class Student : INotifyPropertyChanged
    {
        private string _firstname;
        private string _lastName;
        private int _birthYear;
        private int _course;

        public string FirstName
        {
            get { return _firstname; }
            set 
            {
                if (_firstname == value)
                    return;
                _firstname = value;
                _IfPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName == value)
                    return;
                _lastName = value;
                _IfPropertyChanged("LastName");
            }
        }

        public int BirthYear
        {
            get { return _birthYear; }
            set
            {
                if (_birthYear == value)
                    return;
                _birthYear = value;
                _IfPropertyChanged("BirthYear");
            }
        }

        public int Course
        {
            get { return _course; }
            set
            {
                if (_course == value)
                    return;
                _course = value;
                _IfPropertyChanged("Course");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void _IfPropertyChanged(string nameProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameProperty));
        }
    }
}
