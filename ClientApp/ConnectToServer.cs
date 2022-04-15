using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Text.Json;
using System.ComponentModel;
using System;

namespace ClientApp
{
    /// <summary>
    /// Устанавливает подключение к серверу через Socket
    /// </summary>
    public static class ConnectToServer
    {
        private static int _port = 8080;
        private static string _ip = "127.0.0.1";
        private static bool _isCorrect = true;

        #region Public:
        public static string IP
        {
            get { return _ip; }
            set 
            {
                IPAddress forCheck;
                if (IPAddress.TryParse(value, out forCheck))
                {
                    _ip = value;
                }
                else
                {
                    MessageBox.Show("Неверно указан IP-адрес!");
                    IsCorrect = false;
                }

            }
        }

        public static string Port
        {
            get { return _port.ToString(); }
            set 
            {
                int x;
                if (int.TryParse(value, out x))
                {
                    _port = int.Parse(value);
                }
                else
                {
                    MessageBox.Show("Неверно указан порт!");
                    IsCorrect = false;
                }
                 
            }
        }

        public static bool IsCorrect { get => _isCorrect; set => _isCorrect = value; }

        public static void SendMessage(string message,ref BindingList<Student> list )
        {
            try
            {
                Socket socket = _ToConnect();

                if (socket != null)
                {
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    socket.Send(data);

                    list = _ReceiveData(socket);

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion // Public
        #region Private:
        /// <summary>
        /// Обрабатывает поступаемую информацию от сервера
        /// </summary>
        private static BindingList<Student> _ReceiveData(Socket socket)
        {
            // Этап согласованно получения данных:
            // 1. получение информации о размере данных (одно целое число типа Int32)
            byte[] size = new byte[4];
            socket.Receive(size);

            // 2. получения данных в сериализованном виде (коллекция типа List<Student>)
            byte[] data = new byte[BitConverter.ToInt32(size, 0)];
            socket.Receive(data);

            if (BitConverter.ToInt32(size, 0) > 0)
                return JsonSerializer.Deserialize<BindingList<Student>>(data);
            else
                return new BindingList<Student>();
        }

        /// <summary>
        /// Устанавливает соединение с сервером и уведомляет о результате. 
        /// В случае неудачи возвращает null
        /// </summary>
        private static Socket _ToConnect()
        {
            if (IsCorrect)
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                MessageBox.Show("Соединение установлено!");
                return socket;
            }
            else
            {
                MessageBox.Show("Соединение не установлено!");
                return null;
            }
        }
        #endregion // Private
    }
}
