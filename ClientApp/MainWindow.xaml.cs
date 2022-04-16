using System.Collections.Generic;
using System.Windows;

namespace ClientApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Student> _listMainStudents = new List<Student>(); // основной список, который отображается на экране
        private List<Student> _listToSaveStudents = new List<Student>(); // список для хранения новых объектов

        public MainWindow()
        {
            InitializeComponent();
            textBoxIp.Text = ConnectToServer.IP;
            textBoxPort.Text = ConnectToServer.Port;
        }

        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer.IP = textBoxIp.Text;
            ConnectToServer.Port = textBoxPort.Text;

            if (ConnectToServer.IsCorrect)
            {
                _listMainStudents = ConnectToServer.ReceiveData("connect");
                dgList.ItemsSource = _listMainStudents;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            nameTextBox.Text = "";
            lastnameTextBox.Text = "";
            yearTextBox.Text = "";
            courseTextBox.Text = "";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Student student = new Student
            {
                FirstName = nameTextBox.Text,
                LastName = lastnameTextBox.Text
            };

            if (int.TryParse(yearTextBox.Text, out _))
            {
                student.BirthYear = int.Parse(yearTextBox.Text);
            }

            if (int.TryParse(courseTextBox.Text, out _))
            {
                student.Course = int.Parse(courseTextBox.Text);
            }

            dgList.ItemsSource = null;
            _listToSaveStudents.Add(student);
            _listMainStudents.Add(student);

            dgList.ItemsSource = _listMainStudents;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer.SendDate(ref _listToSaveStudents, "save");
        }
    }
}