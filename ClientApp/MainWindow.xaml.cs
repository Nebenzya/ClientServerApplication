using System.Collections.Generic;
using System.Windows;

namespace ClientApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Student> _listToSaveStudents; // для хранения всего списка из базы данных
        //private List<Student> _listToSendStudents; // для хранения изменений

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
                ConnectToServer.SendMessage("connect",ref _listToSaveStudents);
                dgList.ItemsSource = _listToSaveStudents;
            }
        }
    }
}
