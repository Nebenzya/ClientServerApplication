using System.ComponentModel;
using System.Windows;

namespace ClientApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BindingList<Student> _ListStudents = new BindingList<Student> { new Student() { } };

        public MainWindow()
        {
            InitializeComponent();
            textBoxIp.Text = ConnectToServer.IP;
            textBoxPort.Text = ConnectToServer.Port;
        }
        private void Exit_Menu(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgList.ItemsSource = _ListStudents;
            _ListStudents.ListChanged += _ListStudents_ListChanged;
        }

        private void _ListStudents_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemChanged || e.ListChangedType == ListChangedType.ItemDeleted)
            {
                
            }
        }

        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer.IP = textBoxIp.Text;
            ConnectToServer.Port = textBoxPort.Text;

            if (ConnectToServer.IsCorrect)
            {
                ConnectToServer.SendMessage("connect",ref _ListStudents);
            }
        }
    }
}
