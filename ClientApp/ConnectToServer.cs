using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Text.Json;
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
                if (IPAddress.TryParse(value, out _))
                {
                    _ip = value;
                    _isCorrect = true;
                }
                else
                {
                    MessageBox.Show("Неверно указан IP-адрес!");
                    _isCorrect = false;
                }

            }
        }

        public static string Port
        {
            get { return _port.ToString(); }
            set 
            {
                if (int.TryParse(value, out _))
                {
                    _port = int.Parse(value);
                    _isCorrect = true;
                }
                else
                {
                    MessageBox.Show("Неверно указан порт!");
                    _isCorrect = false;
                }
                 
            }
        }

        public static bool IsCorrect { get => _isCorrect; }

        public static void SendMessage(string message,ref List<Student> listStudents )
        {
            try
            {
                Socket socket = ToConnect();

                if (socket != null)
                {
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    socket.Send(data);

                    listStudents = ReceiveData(socket);

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
        private static List<Student> ReceiveData(Socket socket)
        {
            // Этап согласованно приёма данных:
            // 1. получение информации о размере данных (одно целое число типа Int32)
            byte[] size = new byte[4];
            socket.Receive(size);

            // 2. получения данных в сериализованном виде (коллекция типа List<Student>)
            byte[] data = new byte[BitConverter.ToInt32(size, 0)];
            socket.Receive(data);

            if (BitConverter.ToInt32(size, 0) > 0)
                return JsonSerializer.Deserialize<List<Student>>(data);
            else
                return new List<Student>();
        }

        /// <summary>
        /// Устанавливает соединение с сервером и уведомляет о результате. 
        /// В случае неудачи возвращает null
        /// </summary>
        private static Socket ToConnect()
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
